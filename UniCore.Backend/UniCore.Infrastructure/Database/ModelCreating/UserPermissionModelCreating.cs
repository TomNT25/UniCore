using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserPermissionModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("user_permissions");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("user_id").HasMaxLength(50);
                entity.Property(e => e.PermissionId).HasColumnName("permission_id").HasMaxLength(50);

                entity.Property(e => e.AssignedAt)
                    .HasColumnName("assigned_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.HasIndex(e => new { e.UserId, e.PermissionId }).IsUnique().HasDatabaseName("UQ_user_permissions_user_permission");
                entity.HasIndex(e => e.PermissionId).HasDatabaseName("IX_user_permissions_permission_id");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserPermissions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Permission)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(e => e.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
