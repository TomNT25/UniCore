using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using UniCore.Application.Contract.External;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Auth.Login;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;
using UserEntity = UniCore.Application.Entity.User;

namespace UniCore.Application.Feature.v1.Auth.GoogleAuth.GoogleLogin
{
    public class GoogleLoginHandler : IRequestHandler<GoogleLoginRequestDTO, LoginResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserExternalLoginRepository _userExternalLoginRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IGoogleAuthProviderClient _googleAuthProviderClient;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IValidator<GoogleLoginRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public GoogleLoginHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IUserProfileRepository userProfileRepository,
            IUserExternalLoginRepository userExternalLoginRepository,
            IRoleRepository roleRepository,
            IUserTokenRepository userTokenRepository,
            IGoogleAuthProviderClient googleAuthProviderClient,
            IJwtService jwtService,
            IPasswordHasherService passwordHasherService,
            IMapper mapper,
            IConfiguration configuration,
            IValidator<GoogleLoginRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userProfileRepository = userProfileRepository;
            _userExternalLoginRepository = userExternalLoginRepository;
            _roleRepository = roleRepository;
            _userTokenRepository = userTokenRepository;
            _googleAuthProviderClient = googleAuthProviderClient;
            _jwtService = jwtService;
            _passwordHasherService = passwordHasherService;
            _mapper = mapper;
            _configuration = configuration;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<LoginResponseDTO> HandleAsync(GoogleLoginRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            // Verify Google ID Token via HttpClient call to 3rd-party Google Provider API
            var googleUserInfo = await _googleAuthProviderClient.VerifyIdTokenAsync(request.IdToken, cancellationToken);
            if (googleUserInfo == null || !googleUserInfo.IsValid || string.IsNullOrWhiteSpace(googleUserInfo.Email) || string.IsNullOrWhiteSpace(googleUserInfo.Sub))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.InvalidGoogleToken);
                throw new UnauthorizedAccessException(errMessage);
            }

            UserEntity? userEntity = null;

            var externalLogin = await _userExternalLoginRepository.GetByProviderAndProviderUserIdAsync("google", googleUserInfo.Sub, cancellationToken);
            if (externalLogin != null)
            {
                userEntity = await _userRepository.GetByIdAsync(externalLogin.UserId, cancellationToken);
            }

            if (userEntity == null)
            {
                userEntity = await _userRepository.GetByEmailAsync(googleUserInfo.Email, cancellationToken);
                if (userEntity != null)
                {
                    // Link external Google login to existing user account
                    if (externalLogin == null)
                    {
                        var newExternal = new UserExternalLogin
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = userEntity.Id,
                            Provider = "google",
                            ProviderUserId = googleUserInfo.Sub,
                            ProviderEmail = googleUserInfo.Email,
                            ProviderDisplayName = googleUserInfo.Name,
                            AvatarUrl = googleUserInfo.Picture,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _userExternalLoginRepository.AddAsync(newExternal, cancellationToken);
                    }
                }
                else
                {
                    // Create brand new User with Google provider
                    var defaultRole = await _roleRepository.GetDefaultRoleAsync(cancellationToken);
                    if (defaultRole == null)
                    {
                        var roleMsg = _localizer.GetString(MessageConstants.Auth.DefaultRoleNotFound);
                        throw new InvalidOperationException(roleMsg);
                    }

                    var newUserId = Guid.NewGuid().ToString();
                    userEntity = new UserEntity
                    {
                        Id = newUserId,
                        Username = googleUserInfo.Email,
                        Email = googleUserInfo.Email,
                        PasswordHash = _passwordHasherService.HashPassword(Guid.NewGuid().ToString()),
                        Provider = "google",
                        IsActive = true,
                        IsEmailVerified = true,
                        EmailVerifiedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _userRepository.AddAsync(userEntity, cancellationToken);

                    var profile = new UserProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = newUserId,
                        FullName = googleUserInfo.Name,
                        AvatarUrl = googleUserInfo.Picture,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _userProfileRepository.AddAsync(profile, cancellationToken);

                    var newExternal = new UserExternalLogin
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = newUserId,
                        Provider = "google",
                        ProviderUserId = googleUserInfo.Sub,
                        ProviderEmail = googleUserInfo.Email,
                        ProviderDisplayName = googleUserInfo.Name,
                        AvatarUrl = googleUserInfo.Picture,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _userExternalLoginRepository.AddAsync(newExternal, cancellationToken);

                    var userRole = new UserRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = newUserId,
                        RoleId = defaultRole.Id,
                        AssignedAt = DateTime.UtcNow,
                        IsActive = true,
                        Role = defaultRole
                    };
                    await _userRoleRepository.AddAsync(userRole, cancellationToken);

                    userEntity.UserProfile = profile;
                    userEntity.UserRoles = new List<UserRole> { userRole };
                }
            }

            var userDto = _mapper.Map<UserDTO>(userEntity);
            var accessToken = _jwtService.GenerateAccessToken(userDto);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var tokenExpireMinutes = int.Parse(_configuration[AuthConstants.JwtConfig.TokenExpireMinutesPath] ?? AuthConstants.JwtConfig.DefaultTokenExpireMinutes.ToString());
            var refreshTokenExpireDays = int.Parse(_configuration[AuthConstants.JwtConfig.RefreshTokenExpireDaysPath] ?? AuthConstants.JwtConfig.DefaultRefreshTokenExpireDays.ToString());

            var userToken = new UserToken
            {
                UserId = userDto.Id,
                RefreshToken = refreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpireDays),
                IsActive = true
            };
            await _userTokenRepository.AddAsync(userToken, cancellationToken);
            await _userTokenRepository.SaveChangesAsync(cancellationToken);

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
