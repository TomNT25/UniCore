using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class ScheduleModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedules");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.StudentCourseId).HasColumnName("student_course_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.TimeSlot).HasColumnName("time_slot").IsRequired().HasMaxLength(20);
                entity.Property(e => e.DayOccur).HasColumnName("day_occur").HasColumnType("smalldatetime").IsRequired();

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

                entity.HasIndex(e => e.StudentCourseId)
                    .HasDatabaseName("IX_schedules_student_course_id");

                entity.HasIndex(e => new { e.DayOccur, e.TimeSlot })
                    .HasDatabaseName("IX_schedules_day_occur_time_slot");

                entity.HasIndex(e => new { e.IsDeleted, e.IsActive })
                    .HasDatabaseName("IX_schedules_active_deleted");

                entity.HasOne(e => e.CourseStudent)
                    .WithMany(cs => cs.Schedules)
                    .HasForeignKey(e => e.StudentCourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
