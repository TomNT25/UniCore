using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.ForgotPassword
{
    public class ForgotPasswordRequestDTO : IRequest<ForgotPasswordResponseDTO>
    {
        public string Email { get; set; } = string.Empty;
    }
}
