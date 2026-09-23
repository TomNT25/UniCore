namespace UniCore.Helper.Constant
{
    public static class FaceAuthConstants
    {
        /// <summary>
        /// Face enrollment status values for user_face_profiles.status
        /// </summary>
        public static class Status
        {
            /// <summary>User has not enrolled face biometrics.</summary>
            public const string NotEnrolled = "FACE_NOT_ENROLLED";

            /// <summary>Face images submitted to AI; awaiting PIN setup.</summary>
            public const string PendingPin = "FACE_PENDING_PIN";

            /// <summary>Face enrolled and PIN set; ready for face login.</summary>
            public const string Enrolled = "FACE_ENROLLED";

            /// <summary>Account suspended due to security concerns (e.g., too many failed attempts).</summary>
            public const string Suspended = "FACE_SUSPENDED";
        }

        /// <summary>
        /// Error codes returned by Face AI or mapped by BE.
        /// </summary>
        public static class ErrorCodes
        {
            // From Face AI pipeline
            public const string ImageReadFailed = "IMAGE_READ_FAILED";
            public const string ImageTooSmall = "IMAGE_TOO_SMALL";
            public const string ImageTooDark = "IMAGE_TOO_DARK";
            public const string ImageTooBright = "IMAGE_TOO_BRIGHT";
            public const string ImageTooBlurry = "IMAGE_TOO_BLURRY";
            public const string NoFaceDetected = "NO_FACE_DETECTED";
            public const string MultipleFaces = "MULTIPLE_FACES";
            public const string FaceTooSmall = "FACE_TOO_SMALL";
            public const string LowDetectionConfidence = "LOW_DETECTION_CONFIDENCE";
            public const string ExcessiveYaw = "EXCESSIVE_YAW";
            public const string ExcessivePitch = "EXCESSIVE_PITCH";
            public const string ExcessiveRoll = "EXCESSIVE_ROLL";
            public const string SpoofDetected = "SPOOF_DETECTED";
            public const string InsufficientValidImages = "INSUFFICIENT_VALID_IMAGES";
            public const string FusionFailed = "FUSION_FAILED";
            public const string NoCandidates = "NO_CANDIDATES";

            // BE-level errors
            public const string InternalError = "INTERNAL_ERROR";
            public const string InvalidImageType = "INVALID_IMAGE_TYPE";
            public const string ImageTooLarge = "IMAGE_TOO_LARGE";
            public const string MissingImages = "MISSING_IMAGES";
            public const string AlreadyEnrolled = "ALREADY_ENROLLED";
            public const string NotEnrolled = "NOT_ENROLLED";
            public const string PinRequired = "PIN_REQUIRED";
            public const string InvalidPin = "INVALID_PIN";
            public const string PinLocked = "PIN_LOCKED";
            public const string AccountSuspended = "ACCOUNT_SUSPENDED";
            public const string NoMatch = "NO_MATCH";
            public const string RateLimited = "RATE_LIMITED";
            public const string ChallengeExpired = "CHALLENGE_EXPIRED";
            public const string InvalidChallenge = "INVALID_CHALLENGE";
        }

        /// <summary>
        /// PIN-related configuration.
        /// </summary>
        public static class Pin
        {
            public const int Length = 6;
            public const int MaxFailedAttempts = 5;
            public const int LockoutMinutes = 15;
        }

        /// <summary>
        /// Allowed content types for face images.
        /// </summary>
        public static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/bmp",
            "image/webp"
        };
    }
}
