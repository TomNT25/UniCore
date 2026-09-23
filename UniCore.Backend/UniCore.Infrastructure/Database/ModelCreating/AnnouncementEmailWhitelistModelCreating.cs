using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class AnnouncementEmailWhitelistModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnouncementEmailWhitelist>(entity =>
            {
                entity.ToTable("announcement_email_whitelists");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
                entity.Property(e => e.Type).HasColumnName("type").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Value).HasColumnName("value").IsRequired().HasMaxLength(255);
                entity.Property(e => e.Status).HasColumnName("status").IsRequired().HasMaxLength(20).HasDefaultValue("ACTIVE");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("sysutcdatetime()");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

                entity.HasIndex(e => new { e.Type, e.Value }).IsUnique();

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Updater)
                    .WithMany()
                    .HasForeignKey(e => e.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
