namespace UniCore.Application.Feature.v1.Auth.GoogleAuth.SimulateGoogleToken
{
    public class SimulateGoogleTokenResponseDTO
    {
        public string IdToken { get; set; } = string.Empty;
        public string Sub { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Picture { get; set; }
    }
}
