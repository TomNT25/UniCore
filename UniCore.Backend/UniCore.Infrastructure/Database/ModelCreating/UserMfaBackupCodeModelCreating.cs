using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserMfaBackupCodeModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMfaBackupCode>(entity =>
            {
                entity.ToTable("user_mfa_backup_codes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.CodeHash).HasColumnName("code_hash").IsRequired().HasMaxLength(255);
                entity.Property(e => e.IsUsed).HasColumnName("is_used").HasDefaultValue(false);
                entity.Property(e => e.UsedAt).HasColumnName("used_at");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => new { e.UserId, e.IsUsed })
                    .HasDatabaseName("IX_user_mfa_backup_codes_user_id")
                    .IncludeProperties(e => e.CodeHash);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserMfaBackupCodes)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
