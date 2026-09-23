namespace UniCore.Application.Entity
{
    public class UserMfaBackupCode
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public string CodeHash { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; } = null!;
    }
}
