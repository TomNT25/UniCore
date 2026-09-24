namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.EnableFaceMfa
{
    public class EnableFaceMfaResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string MfaMethod { get; set; } = "FACE";
        public DateTime? EnabledAt { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
