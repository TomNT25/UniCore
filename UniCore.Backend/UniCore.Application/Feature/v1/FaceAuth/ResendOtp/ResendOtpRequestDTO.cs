using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.ResendOtp
{
    public class ResendOtpRequestDTO : IRequest<ResendOtpResponseDTO>
    {
        public string ChallengeToken { get; set; } = string.Empty;
    }
}
