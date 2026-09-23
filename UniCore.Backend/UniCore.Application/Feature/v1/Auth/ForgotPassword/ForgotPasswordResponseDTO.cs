namespace UniCore.Application.Feature.v1.Auth.ForgotPassword
{
    public class ForgotPasswordResponseDTO
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
        public string Message { get; set; } = "Password reset OTP generated successfully";
    }
}
