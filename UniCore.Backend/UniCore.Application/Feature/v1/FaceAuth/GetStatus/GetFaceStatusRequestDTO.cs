using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.GetStatus
{
    /// <summary>
    /// Request DTO for getting face auth status.
    /// </summary>
    public class GetFaceStatusRequestDTO : IRequest<GetFaceStatusResponseDTO>
    {
        /// <summary>
        /// User ID from JWT token (set by controller).
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
