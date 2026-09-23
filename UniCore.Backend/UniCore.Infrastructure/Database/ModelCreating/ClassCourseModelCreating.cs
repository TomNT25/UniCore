using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class ClassCourseModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassCourse>(entity =>
            {
                entity.ToTable("class_courses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.CourseId).HasColumnName("course_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.SchoolClassId).HasColumnName("student_id").IsRequired().HasMaxLength(50);

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

                entity.HasIndex(e => new { e.CourseId, e.SchoolClassId }).IsUnique();

                entity.HasOne(e => e.SchoolClass)
                    .WithMany(c => c.ClassCourses)
                    .HasForeignKey(e => e.SchoolClassId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Course)
                    .WithMany(s => s.ClassCourses)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
