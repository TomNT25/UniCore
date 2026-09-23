namespace UniCore.Application.Entity;

public class RolePermission
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Code { get; set; }
    public string RoleId { get; set; } = string.Empty;
    public string PermissionId { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string? AssignedBy { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}
