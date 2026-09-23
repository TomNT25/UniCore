using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserFaceProfileModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFaceProfile>(entity =>
            {
                entity.ToTable("user_face_profiles");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDefaultValue("FACE_NOT_ENROLLED");

                entity.Property(e => e.EmbeddingId)
                    .HasColumnName("embedding_id")
                    .HasMaxLength(100);

                entity.Property(e => e.ModelVersion)
                    .HasColumnName("model_version")
                    .HasMaxLength(50);

                entity.Property(e => e.PinHash)
                    .HasColumnName("pin_hash")
                    .HasMaxLength(100);

                entity.Property(e => e.FailedPinAttempts)
                    .HasColumnName("failed_pin_attempts")
                    .HasDefaultValue(0);

                entity.Property(e => e.PinLockoutEnd)
                    .HasColumnName("pin_lockout_end");

                entity.Property(e => e.EnrolledAt)
                    .HasColumnName("enrolled_at");

                entity.Property(e => e.PinSetAt)
                    .HasColumnName("pin_set_at");

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

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(50);

                // One user can have one face profile
                entity.HasIndex(e => e.UserId).IsUnique();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<UserFaceProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
