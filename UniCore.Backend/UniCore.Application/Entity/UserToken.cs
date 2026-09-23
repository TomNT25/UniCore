namespace UniCore.Application.Entity
{
    public class UserToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string? Jti { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(30);
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual User User { get; set; } = null!;
    }
}