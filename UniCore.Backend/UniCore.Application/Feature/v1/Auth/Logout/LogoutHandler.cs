using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutRequestDTO, LogoutResponseDTO>
    {
        private readonly IUserTokenRepository _userTokenRepository;

        public LogoutHandler(IUserTokenRepository userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }

        public async Task<LogoutResponseDTO> HandleAsync(LogoutRequestDTO request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                await _userTokenRepository.RevokeTokenAsync(request.RefreshToken, cancellationToken);
            }

            return new LogoutResponseDTO
            {
                Success = true,
                Message = "Logged out successfully"
            };
        }
    }
}
