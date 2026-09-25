using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserProfileModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("user_profiles", tb => tb.UseSqlOutputClause(false));
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100);
                entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
                entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
                entity.Property(e => e.AvatarMediaFileId).HasColumnName("avatar_media_file_id");
                entity.Property(e => e.CidFrontImageUrl).HasColumnName("cid_front_image_url");
                entity.Property(e => e.CidBackImageUrl).HasColumnName("cid_back_image_url");
                entity.Property(e => e.Gender).HasColumnName("gender").HasMaxLength(20);
                entity.Property(e => e.BirthDate).HasColumnName("birth_date");
                entity.Property(e => e.Address).HasColumnName("address");
                entity.Property(e => e.Bio).HasColumnName("bio").HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.Property(e => e.IsVerified)
                    .HasColumnName("is_verified")
                    .HasDefaultValue(false);

                entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");
                entity.Property(e => e.VerifiedBy).HasColumnName("verified_by").HasMaxLength(50);

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
                
                entity.HasIndex(e => e.UserId).IsUnique().HasDatabaseName("UQ_user_profiles_user_id");
                entity.HasIndex(e => e.PhoneNumber).HasDatabaseName("IX_user_profiles_phone_number").HasFilter("[phone_number] IS NOT NULL");
                entity.HasIndex(e => e.FullName).HasDatabaseName("IX_user_profiles_full_name").HasFilter("[full_name] IS NOT NULL");

                entity.HasOne(e => e.User)
                    .WithOne(u => u.UserProfile)
                    .HasForeignKey<UserProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
