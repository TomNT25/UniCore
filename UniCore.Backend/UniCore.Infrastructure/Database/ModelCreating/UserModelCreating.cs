using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.StudentCode).HasColumnName("student_code").HasMaxLength(50);
                entity.Property(e => e.ClassId).HasColumnName("class_id").HasMaxLength(50);
                entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(255);

                entity.Property(e => e.Provider)
                    .HasColumnName("provider")
                    .HasMaxLength(50)
                    .HasDefaultValue("system");

                entity.Property(e => e.FailedLoginAttempts)
                    .HasColumnName("failed_login_attempts")
                    .HasDefaultValue(0);

                entity.Property(e => e.LockoutEnd).HasColumnName("lockout_end");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.Property(e => e.IsEmailVerified)
                    .HasColumnName("is_email_verified")
                    .HasDefaultValue(false);

                entity.Property(e => e.EmailVerifiedAt).HasColumnName("email_verified_at");
                entity.Property(e => e.LastLoginAt).HasColumnName("last_login_at");

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

                entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("UQ_users_email");
                entity.HasIndex(e => e.Username).IsUnique().HasDatabaseName("UQ_users_username");
                entity.HasIndex(e => e.ClassId).HasDatabaseName("IX_users_class_id").HasFilter("[class_id] IS NOT NULL");
                entity.HasIndex(e => e.StudentCode).HasDatabaseName("IX_users_student_code").HasFilter("[student_code] IS NOT NULL");
                entity.HasIndex(e => e.Code).HasDatabaseName("IX_users_code").HasFilter("[code] IS NOT NULL");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_users_is_active");

                entity.HasOne(e => e.Class)
                    .WithMany(c => c.Students)
                    .HasForeignKey(e => e.ClassId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(x => x.UserRoles)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);

                entity.HasMany(x => x.UserPermissions)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);
            });
        }
    }
}
