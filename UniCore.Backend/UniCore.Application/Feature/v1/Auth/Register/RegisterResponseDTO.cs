namespace UniCore.Application.Feature.v1.Auth.Register
{
    public class RegisterResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? OtpCode { get; set; }
    }
}
