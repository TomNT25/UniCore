namespace UniCore.Application.Entity;

public class Announcement
{
    public string Id { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Type { get; set; } = "NORMAL";
    public string Status { get; set; } = "DRAFT";
    public string ScopeType { get; set; } = "ALL";
    public string? ScopeValue { get; set; }
    public bool RequireAcknowledgement { get; set; } = false;
    public DateTime? PublishDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public int? RecipientCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public virtual User? Creator { get; set; }
    public virtual User? Updater { get; set; }

    public virtual ICollection<AnnouncementStudent> AnnouncementStudents { get; set; } = new List<AnnouncementStudent>();
    public virtual ICollection<AnnouncementEmailLog> AnnouncementEmailLogs { get; set; } = new List<AnnouncementEmailLog>();
}
