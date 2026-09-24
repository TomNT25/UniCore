using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyOtp
{
    public class VerifyOtpRequestDTO : IRequest<VerifyOtpResponseDTO>
    {
        public string ChallengeToken { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
