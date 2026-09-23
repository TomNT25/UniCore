using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Auth.Login;

namespace UniCore.Application.Feature.v1.Auth.GoogleAuth.GoogleLogin
{
    public class GoogleLoginRequestDTO : IRequest<LoginResponseDTO>
    {
        public string IdToken { get; set; } = string.Empty;
    }
}
