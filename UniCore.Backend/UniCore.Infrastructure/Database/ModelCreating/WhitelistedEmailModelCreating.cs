using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class WhitelistedEmailModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WhitelistedEmail>(entity =>
            {
                entity.ToTable("whitelisted_emails");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsConfirmed).HasColumnName("is_confirmed").HasDefaultValue(false);
                entity.Property(e => e.StudentId).HasColumnName("student_id").IsRequired().HasMaxLength(50);

                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

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

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("UQ_whitelisted_emails_email");

                entity.HasIndex(e => e.StudentId)
                    .HasDatabaseName("IX_whitelisted_emails_student_id");

                entity.HasIndex(e => new { e.IsConfirmed, e.IsActive, e.IsDeleted })
                    .HasDatabaseName("IX_whitelisted_emails_confirmed_active");

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.WhitelistedEmails)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
