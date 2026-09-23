using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserExternalLoginModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserExternalLogin>(entity =>
            {
                entity.ToTable("user_external_logins");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Provider).HasColumnName("provider").IsRequired().HasMaxLength(50);
                entity.Property(e => e.ProviderUserId).HasColumnName("provider_user_id").IsRequired().HasMaxLength(255);
                entity.Property(e => e.ProviderEmail).HasColumnName("provider_email").HasMaxLength(100);
                entity.Property(e => e.ProviderDisplayName).HasColumnName("provider_display_name").HasMaxLength(200);
                entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
                entity.Property(e => e.AccessToken).HasColumnName("access_token");
                entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
                entity.Property(e => e.TokenExpiresAt).HasColumnName("token_expires_at");

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

                entity.HasIndex(e => new { e.Provider, e.ProviderUserId })
                    .IsUnique()
                    .HasDatabaseName("UQ_user_external_logins_provider_key");

                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_user_external_logins_user_id");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserExternalLogins)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
