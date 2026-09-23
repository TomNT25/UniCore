namespace UniCore.Application.Entity
{
    public class UserMfaSetting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string MfaMethod { get; set; } = "TOTP";
        public string? SecretKey { get; set; }
        public bool IsMfaEnabled { get; set; } = false;
        public DateTime? EnabledAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
