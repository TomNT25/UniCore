namespace UniCore.Application.Entity;

public class AnnouncementEmailLog
{
    public string Id { get; set; } = string.Empty;
    public string AnnouncementId { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty; // Maps to User.Id
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = "PENDING";
    public DateTime? SentAt { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Announcement Announcement { get; set; } = null!;
    public virtual User Student { get; set; } = null!;
}
