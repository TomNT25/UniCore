namespace UniCore.Application.Feature.v1.Auth.ForgotPassword
{
    public class ResetPasswordResponseDTO
    {
        public string Email { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Password reset successfully";
    }
}
