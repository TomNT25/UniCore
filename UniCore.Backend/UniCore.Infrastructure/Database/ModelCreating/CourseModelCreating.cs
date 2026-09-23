using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class CourseModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.Type).HasColumnName("type").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
                entity.Property(e => e.DepartmentId).HasColumnName("department_id").IsRequired().HasMaxLength(50);

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

                entity.HasIndex(e => e.Code)
                    .IsUnique()
                    .HasDatabaseName("UQ_courses_code");

                entity.HasIndex(e => e.DepartmentId)
                    .HasDatabaseName("IX_courses_department_id");

                entity.HasIndex(e => e.Type)
                    .HasDatabaseName("IX_courses_type");

                entity.HasIndex(e => new { e.IsDeleted, e.IsActive })
                    .HasDatabaseName("IX_courses_active_deleted");

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Courses)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
