using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth
{
    /// <summary>
    /// Maps Face AI error codes to user-friendly messages.
    /// Follows doc §6 error code specification.
    /// </summary>
    public static class FaceAuthErrorMapper
    {
        /// <summary>
        /// Map Face AI error code to client-facing error code.
        /// Some codes are normalized for security (e.g., NO_CANDIDATES → NO_MATCH).
        /// </summary>
        public static string MapToClientCode(string? errorCode)
        {
            return errorCode switch
            {
                // Direct mappings (safe to expose)
                FaceAuthConstants.ErrorCodes.ImageReadFailed => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.ImageTooSmall => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.ImageTooDark => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.ImageTooBright => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.ImageTooBlurry => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.FaceTooSmall => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.LowDetectionConfidence => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.ExcessiveYaw => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.ExcessivePitch => "LOW_QUALITY",
                FaceAuthConstants.ErrorCodes.ExcessiveRoll => "LOW_QUALITY",

                FaceAuthConstants.ErrorCodes.NoFaceDetected => "NO_FACE",
                FaceAuthConstants.ErrorCodes.MultipleFaces => "MULTIPLE_FACES",
                FaceAuthConstants.ErrorCodes.SpoofDetected => "LIVENESS_FAILED",
                FaceAuthConstants.ErrorCodes.InsufficientValidImages => "INSUFFICIENT_IMAGES",
                FaceAuthConstants.ErrorCodes.FusionFailed => "ENROLLMENT_FAILED",

                // Security-sensitive mappings (don't leak user existence)
                FaceAuthConstants.ErrorCodes.NoCandidates => "NO_MATCH",
                FaceAuthConstants.ErrorCodes.NoMatch => "NO_MATCH",

                // BE-level errors
                FaceAuthConstants.ErrorCodes.InvalidImageType => "INVALID_IMAGE",
                FaceAuthConstants.ErrorCodes.ImageTooLarge => "INVALID_IMAGE",
                FaceAuthConstants.ErrorCodes.MissingImages => "MISSING_IMAGES",
                FaceAuthConstants.ErrorCodes.AlreadyEnrolled => "ALREADY_ENROLLED",
                FaceAuthConstants.ErrorCodes.NotEnrolled => "NOT_ENROLLED",
                FaceAuthConstants.ErrorCodes.PinRequired => "PIN_REQUIRED",
                FaceAuthConstants.ErrorCodes.InvalidPin => "INVALID_PIN",
                FaceAuthConstants.ErrorCodes.PinLocked => "PIN_LOCKED",
                FaceAuthConstants.ErrorCodes.AccountSuspended => "ACCOUNT_SUSPENDED",
                FaceAuthConstants.ErrorCodes.InternalError => "INTERNAL_ERROR",

                _ => "UNKNOWN_ERROR"
            };
        }

        /// <summary>
        /// Get user-friendly message for client error code.
        /// </summary>
        public static string GetClientMessage(string clientCode)
        {
            return clientCode switch
            {
                "LOW_QUALITY" => "Image quality is too low. Please ensure good lighting and hold the camera steady.",
                "NO_FACE" => "No face detected. Please ensure your face is clearly visible.",
                "MULTIPLE_FACES" => "Multiple faces detected. Please ensure only one face is in the frame.",
                "LIVENESS_FAILED" => "Liveness check failed. Please use a live camera feed, not a photo.",
                "INSUFFICIENT_IMAGES" => "Not enough valid images. Please try again with clearer photos.",
                "ENROLLMENT_FAILED" => "Face enrollment failed. Please try again.",
                "NO_MATCH" => "No matching face found. Please try again or use password login.",
                "INVALID_IMAGE" => "Invalid image format or size. Please use JPEG or PNG under 10MB.",
                "MISSING_IMAGES" => "Face image is required.",
                "ALREADY_ENROLLED" => "Face is already enrolled for this account.",
                "NOT_ENROLLED" => "Face authentication is not set up. Please enroll first.",
                "PIN_REQUIRED" => "PIN is required to complete face login.",
                "INVALID_PIN" => "Invalid PIN. Please try again.",
                "PIN_LOCKED" => "Too many failed attempts. Please try again later.",
                "ACCOUNT_SUSPENDED" => "Face authentication is suspended for this account.",
                "RATE_LIMITED" => "Too many requests. Please wait and try again.",
                "INTERNAL_ERROR" => "An error occurred. Please try again later.",
                _ => "An unexpected error occurred. Please try again."
            };
        }

        /// <summary>
        /// Map and get both client code and message.
        /// </summary>
        public static (string Code, string Message) MapError(string? aiErrorCode)
        {
            var code = MapToClientCode(aiErrorCode);
            return (code, GetClientMessage(code));
        }
    }
}
