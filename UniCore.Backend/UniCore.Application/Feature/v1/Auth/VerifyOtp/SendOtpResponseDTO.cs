namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class SendOtpResponseDTO
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
        public string Message { get; set; } = "OTP sent successfully to email";
    }
}
