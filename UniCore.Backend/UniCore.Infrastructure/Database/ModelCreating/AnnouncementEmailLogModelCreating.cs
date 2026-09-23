using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class AnnouncementEmailLogModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnouncementEmailLog>(entity =>
            {
                entity.ToTable("announcement_email_logs");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");
                entity.Property(e => e.AnnouncementId).HasColumnName("announcement_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.StudentId).HasColumnName("student_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).HasColumnName("status").IsRequired().HasMaxLength(20).HasDefaultValue("PENDING");
                entity.Property(e => e.SentAt).HasColumnName("sent_at");
                entity.Property(e => e.ErrorMessage).HasColumnName("error_message").HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("sysutcdatetime()");

                entity.HasIndex(e => e.Status)
                    .HasDatabaseName("IX_announcement_email_logs_status");

                entity.HasIndex(e => e.AnnouncementId)
                    .HasDatabaseName("IX_announcement_email_logs_announcement_id");

                entity.HasIndex(e => e.StudentId)
                    .HasDatabaseName("IX_announcement_email_logs_student_id");

                entity.HasOne(e => e.Announcement)
                    .WithMany(a => a.AnnouncementEmailLogs)
                    .HasForeignKey(e => e.AnnouncementId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Student)
                    .WithMany()
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
