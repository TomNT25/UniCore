namespace UniCore.Application.Entity
{
    public class WhitelistedEmail
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; } = false;
        public string StudentId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual User Student { get; set; } = null!;
    }
}
