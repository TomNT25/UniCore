namespace UniCore.Application.Feature.v1.Auth.Mfa.SetupMfa
{
    public class SetupMfaResponseDTO
    {
        public string SecretKey { get; set; } = string.Empty;
        public string QrCodeUri { get; set; } = string.Empty;
        public List<string> BackupCodes { get; set; } = new List<string>();
    }
}
