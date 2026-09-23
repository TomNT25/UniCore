namespace UniCore.Application.Entity;

public class AnnouncementEmailWhitelist
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = "EMAIL";
    public string Value { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public virtual User? Creator { get; set; }
    public virtual User? Updater { get; set; }
}
