using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserRoleModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("user_id").HasMaxLength(50);
                entity.Property(e => e.RoleId).HasColumnName("role_id").HasMaxLength(50);
                entity.Property(e => e.AssignedAt).HasColumnName("assigned_at").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

                entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique().HasDatabaseName("UQ_user_roles_user_role");
                entity.HasIndex(e => e.RoleId).HasDatabaseName("IX_user_roles_role_id");

                entity.HasOne(x => x.User)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Role)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
