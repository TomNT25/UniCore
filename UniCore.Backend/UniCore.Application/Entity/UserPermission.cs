namespace UniCore.Application.Entity;

public class UserPermission
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Code { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string PermissionId { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string? AssignedBy { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual User User { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}
