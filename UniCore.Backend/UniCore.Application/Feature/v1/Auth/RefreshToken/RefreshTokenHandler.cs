using MapsterMapper;
using Microsoft.Extensions.Configuration;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Auth.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequestDTO, RefreshTokenResponseDTO>
    {
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public RefreshTokenHandler(
            IUserTokenRepository userTokenRepository,
            IUserRepository userRepository,
            IJwtService jwtService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userTokenRepository = userTokenRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<RefreshTokenResponseDTO> HandleAsync(RefreshTokenRequestDTO request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidRefreshToken);
            }

            var userToken = await _userTokenRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
            if (userToken == null || !userToken.IsActive || userToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidRefreshToken);
            }

            var user = await _userRepository.GetByIdAsync(userToken.UserId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.UserNotFound);
            }

            // Revoke current refresh token
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            userToken.RevokedAt = DateTime.UtcNow;
            userToken.IsActive = false;
            userToken.ReplacedByToken = newRefreshToken;
            await _userTokenRepository.UpdateAsync(userToken, cancellationToken);

            // Generate new token pair
            var userDto = _mapper.Map<UserDTO>(user);
            var newAccessToken = _jwtService.GenerateAccessToken(userDto);

            var tokenExpireMinutes = int.Parse(_configuration[AuthConstants.JwtConfig.TokenExpireMinutesPath] 
                                              ?? AuthConstants.JwtConfig.DefaultTokenExpireMinutes.ToString());
            var refreshTokenExpireDays = int.Parse(_configuration[AuthConstants.JwtConfig.RefreshTokenExpireDaysPath] 
                                                ?? AuthConstants.JwtConfig.DefaultRefreshTokenExpireDays.ToString());

            // Save new refresh token record
            var newUserToken = new UserToken
            {
                UserId = user.Id,
                RefreshToken = newRefreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpireDays),
                IsActive = true
            };

            await _userTokenRepository.AddAsync(newUserToken, cancellationToken);
            await _userTokenRepository.SaveChangesAsync(cancellationToken);

            return new RefreshTokenResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = tokenExpireMinutes
            };
        }
    }
}
