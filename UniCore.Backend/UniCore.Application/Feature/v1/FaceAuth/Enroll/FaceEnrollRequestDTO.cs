using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.Enroll
{
    /// <summary>
    /// Request DTO for face enrollment.
    /// Images are passed as streams from the controller.
    /// </summary>
    public class FaceEnrollRequestDTO : IRequest<FaceEnrollResponseDTO>
    {
        /// <summary>
        /// User ID from JWT token (set by controller, not from client).
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Username from DB (set by handler based on UserId).
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// List of face images (1-5 images for multi-angle enrollment).
        /// </summary>
        public List<FaceImageInputDTO> FaceImages { get; set; } = new();
    }

    public class FaceImageInputDTO
    {
        public Stream Stream { get; set; } = Stream.Null;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "image/jpeg";
        public long Length { get; set; }
    }
}
