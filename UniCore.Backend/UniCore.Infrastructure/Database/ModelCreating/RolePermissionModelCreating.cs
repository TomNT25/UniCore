using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class RolePermissionModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permissions");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.RoleId).HasColumnName("role_id").HasMaxLength(50);
                entity.Property(e => e.PermissionId).HasColumnName("permission_id").HasMaxLength(50);

                entity.Property(e => e.AssignedAt)
                    .HasColumnName("assigned_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique().HasDatabaseName("UQ_role_permissions_role_permission");
                entity.HasIndex(e => e.PermissionId).HasDatabaseName("IX_role_permissions_permission_id");

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(e => e.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
