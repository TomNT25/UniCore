namespace UniCore.Application.Entity;

public class AnnouncementStudent
{
    public string Id { get; set; } = string.Empty;
    public string AnnouncementId { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public DateTime? ViewedAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    /// <summary>False until client polls GET get-new and BE marks notification as delivered to FE queue.</summary>
    public bool IsSent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Announcement Announcement { get; set; } = null!;
    public virtual User Student { get; set; } = null!;
}
