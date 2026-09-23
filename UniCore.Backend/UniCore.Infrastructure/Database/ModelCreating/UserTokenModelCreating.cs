using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserTokenModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable("user_tokens");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.RefreshToken).HasColumnName("refresh_token").IsRequired();
                entity.Property(e => e.Jti).HasColumnName("jti").HasMaxLength(255);
                entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
                entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);

                entity.Property(e => e.IssuedAt)
                    .HasColumnName("issued_at")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnName("expires_at")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
                entity.Property(e => e.ReplacedByToken).HasColumnName("replaced_by_token");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.HasIndex(e => new { e.UserId, e.IsActive })
                    .HasDatabaseName("IX_user_tokens_user_id");

                entity.HasIndex(e => e.Jti)
                    .HasDatabaseName("IX_user_tokens_jti")
                    .HasFilter("[jti] IS NOT NULL");

                entity.HasIndex(e => e.ExpiresAt)
                    .HasDatabaseName("IX_user_tokens_expires_at");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserTokens)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
