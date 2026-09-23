using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.Login
{
    /// <summary>
    /// Request DTO for face login.
    /// </summary>
    public class FaceLoginRequestDTO : IRequest<FaceLoginResponseDTO>
    {
        /// <summary>
        /// Face image stream from uploaded file.
        /// </summary>
        public Stream FaceStream { get; set; } = Stream.Null;

        /// <summary>
        /// Original file name.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Content type (MIME type).
        /// </summary>
        public string ContentType { get; set; } = "image/jpeg";

        /// <summary>
        /// File length in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Client IP address (set by controller for audit logging).
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User agent string (set by controller for audit logging).
        /// </summary>
        public string? UserAgent { get; set; }
    }
}
