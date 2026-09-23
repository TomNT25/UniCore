using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class AnnouncementModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.ToTable("announcements");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");
                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.Content).HasColumnName("content").IsRequired();
                entity.Property(e => e.Type).HasColumnName("type").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Status).HasColumnName("status").IsRequired().HasMaxLength(20).HasDefaultValue("DRAFT");
                entity.Property(e => e.ScopeType).HasColumnName("scope_type").IsRequired().HasMaxLength(20);
                entity.Property(e => e.ScopeValue).HasColumnName("scope_value").HasMaxLength(50);
                entity.Property(e => e.RequireAcknowledgement).HasColumnName("require_acknowledgement").HasDefaultValue(false);
                entity.Property(e => e.PublishDate).HasColumnName("publish_date");
                entity.Property(e => e.ExpiredDate).HasColumnName("expired_date");
                entity.Property(e => e.RecipientCount).HasColumnName("recipient_count");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("sysutcdatetime()");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

                entity.HasIndex(e => new { e.Status, e.PublishDate })
                    .HasDatabaseName("IX_announcements_status_publish_date");

                entity.HasIndex(e => new { e.ScopeType, e.ScopeValue })
                    .HasDatabaseName("IX_announcements_scope");

                entity.HasIndex(e => e.Code)
                    .HasDatabaseName("IX_announcements_code")
                    .HasFilter("[code] IS NOT NULL");

                entity.HasIndex(e => e.CreatedBy)
                    .HasDatabaseName("IX_announcements_created_by");

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
