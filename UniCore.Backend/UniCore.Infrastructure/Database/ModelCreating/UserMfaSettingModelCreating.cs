using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserMfaSettingModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMfaSetting>(entity =>
            {
                entity.ToTable("user_mfa_settings");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.MfaMethod).HasColumnName("mfa_method").IsRequired().HasMaxLength(30).HasDefaultValue("TOTP");
                entity.Property(e => e.SecretKey).HasColumnName("secret_key").HasMaxLength(255);
                entity.Property(e => e.IsMfaEnabled).HasColumnName("is_mfa_enabled").HasDefaultValue(false);
                entity.Property(e => e.EnabledAt).HasColumnName("enabled_at");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

                entity.HasIndex(e => e.UserId).IsUnique().HasDatabaseName("UQ_user_mfa_settings_user_id");

                entity.HasOne(e => e.User)
                    .WithOne(u => u.UserMfaSetting)
                    .HasForeignKey<UserMfaSetting>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
