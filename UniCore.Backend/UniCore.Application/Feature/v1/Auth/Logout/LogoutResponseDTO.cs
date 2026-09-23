namespace UniCore.Application.Feature.v1.Auth.Logout
{
    public class LogoutResponseDTO
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Logged out successfully";
    }
}
