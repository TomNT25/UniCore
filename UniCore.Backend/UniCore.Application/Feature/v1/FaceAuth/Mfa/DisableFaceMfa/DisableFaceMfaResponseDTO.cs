namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.DisableFaceMfa
{
    public class DisableFaceMfaResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
