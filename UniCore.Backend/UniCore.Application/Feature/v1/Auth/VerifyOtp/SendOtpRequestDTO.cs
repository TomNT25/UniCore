using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class SendOtpRequestDTO : IRequest<SendOtpResponseDTO>
    {
        public string Email { get; set; } = string.Empty;
    }
}
