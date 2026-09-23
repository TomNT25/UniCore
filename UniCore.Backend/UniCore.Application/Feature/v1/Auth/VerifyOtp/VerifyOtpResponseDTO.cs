namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class VerifyOtpResponseDTO
    {
        public string Email { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = true;
        public string Message { get; set; } = "Email verified successfully";
    }
}
