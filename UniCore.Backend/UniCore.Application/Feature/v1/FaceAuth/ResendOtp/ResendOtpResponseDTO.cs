namespace UniCore.Application.Feature.v1.FaceAuth.ResendOtp
{
    public class ResendOtpResponseDTO
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? MaskedEmail { get; set; }
        public string? Message { get; set; }
    }
}
