using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.SetPin
{
    /// <summary>
    /// Request DTO for setting face auth PIN.
    /// </summary>
    public class SetPinRequestDTO : IRequest<SetPinResponseDTO>
    {
        /// <summary>
        /// User ID from JWT token (set by controller).
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 6-digit PIN (never stored in plaintext).
        /// </summary>
        public string Pin { get; set; } = string.Empty;

        /// <summary>
        /// PIN confirmation (must match Pin).
        /// </summary>
        public string ConfirmPin { get; set; } = string.Empty;
    }
}
