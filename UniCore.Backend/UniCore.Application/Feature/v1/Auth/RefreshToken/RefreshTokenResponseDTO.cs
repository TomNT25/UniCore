namespace UniCore.Application.Feature.v1.Auth.RefreshToken
{
    public class RefreshTokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
