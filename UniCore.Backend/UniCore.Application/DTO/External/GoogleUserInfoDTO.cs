namespace UniCore.Application.DTO.External
{
    public class GoogleUserInfoDTO
    {
        public bool IsValid { get; set; }
        public string? Sub { get; set; }
        public string? Email { get; set; }
        public bool EmailVerified { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? Issuer { get; set; }
    }
}
