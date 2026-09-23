using UniCore.Application.DTO.Entity;
using UniCore.Application.Feature.v1.Auth.Login;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyPin
{
    /// <summary>
    /// Response DTO for face login PIN verification.
    /// Returns JWT tokens on success (same as regular login).
    /// </summary>
    public class VerifyPinResponseDTO
    {
        /// <summary>
        /// Whether PIN verification was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// JWT access token (only on success).
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Refresh token (only on success).
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Refresh token expiration in days.
        /// </summary>
        public int? RefreshTokenExpire { get; set; }

        /// <summary>
        /// Basic user info (only on success).
        /// </summary>
        public UserLoginResponseDTO? User { get; set; }

        /// <summary>
        /// Error code if verification failed.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Error message if verification failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Number of remaining PIN attempts before lockout (if failed).
        /// </summary>
        public int? RemainingAttempts { get; set; }
    }
}
