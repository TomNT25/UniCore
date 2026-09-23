namespace UniCore.Application.Entity;

public class AnnouncementStudent
{
    public string Id { get; set; } = string.Empty;
    public string AnnouncementId { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty; // Maps to User.Id
    public DateTime? ViewedAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Announcement Announcement { get; set; } = null!;
    public virtual User Student { get; set; } = null!;
}
