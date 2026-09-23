using MapsterMapper;
using Microsoft.Extensions.Configuration;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginRequestDTO, LoginResponseDTO>
    {
        private readonly GetUserByUsernameQueryHandler _getUserByUsernameQueryHandler;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IConfiguration _configuration;
        private readonly IUserTokenRepository _userTokenRepository;

        public LoginHandler(
            GetUserByUsernameQueryHandler getUserByUsernameQueryHandler,
            IMapper mapper,
            IJwtService jwtService,
            IPasswordHasherService passwordHasherService,
            IConfiguration configuration,
            IUserTokenRepository userTokenRepository)
        {
            _getUserByUsernameQueryHandler = getUserByUsernameQueryHandler;
            _mapper = mapper;
            _jwtService = jwtService;
            _passwordHasherService = passwordHasherService;
            _configuration = configuration;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<LoginResponseDTO> HandleAsync(LoginRequestDTO request, CancellationToken cancellationToken)
        {
            var userEntity = await _getUserByUsernameQueryHandler.HandleAsync(new GetUserByUsernameQuery( request.Username ), cancellationToken);
            if (userEntity == null)
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidCredentials);
            }

            var getUserByUserNameResult = _mapper.Map<UserDTO>(userEntity);

            var passwordMatch = _passwordHasherService.VerifyHashedPassword(getUserByUserNameResult.PasswordHash, request.Password);
            if (!passwordMatch)
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidCredentials);
            }

            var accessToken = _jwtService.GenerateAccessToken(getUserByUserNameResult);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpireDays = int.Parse(_configuration[AuthConstants.JwtConfig.RefreshTokenExpireDaysPath] ?? AuthConstants.JwtConfig.DefaultRefreshTokenExpireDays.ToString());

            var userToken = new UserToken
            {
                UserId = getUserByUserNameResult.Id,
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
                RefreshTokenExpire = refreshTokenExpireDays,
                User = _mapper.Map<UserLoginResponseDTO>(getUserByUserNameResult)
            };
        }
    }
}
