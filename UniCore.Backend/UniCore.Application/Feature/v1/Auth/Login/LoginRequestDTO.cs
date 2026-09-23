using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Login
{
    public class LoginRequestDTO : IRequest<LoginResponseDTO>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
