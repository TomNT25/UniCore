namespace UniCore.Application.Entity
{
    public class UserAuthLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User? User { get; set; }
    }
}
