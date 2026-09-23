using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyPin
{
    /// <summary>
    /// Request DTO for face login PIN verification.
    /// </summary>
    public class VerifyPinRequestDTO : IRequest<VerifyPinResponseDTO>
    {
        /// <summary>
        /// Challenge token from face login response.
        /// </summary>
        public string ChallengeToken { get; set; } = string.Empty;

        /// <summary>
        /// 6-digit PIN.
        /// </summary>
        public string Pin { get; set; } = string.Empty;
    }
}
