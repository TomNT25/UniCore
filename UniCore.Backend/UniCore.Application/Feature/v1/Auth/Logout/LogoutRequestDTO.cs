using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Logout
{
    public class LogoutRequestDTO : IRequest<LogoutResponseDTO>
    {
        public string? RefreshToken { get; set; }
    }
}
