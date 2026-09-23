using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserPersonIdModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPersonId>(entity =>
            {
                entity.ToTable("user_person_ids");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.IdNumber).HasColumnName("id_number").IsRequired().HasMaxLength(20);
                entity.Property(e => e.FullName).HasColumnName("full_name").IsRequired().HasMaxLength(200);
                entity.Property(e => e.CardType).HasColumnName("card_type").IsRequired().HasMaxLength(30).HasDefaultValue("CCCD_CHIP");
                entity.Property(e => e.BirthDate).HasColumnName("birth_date");
                entity.Property(e => e.Gender).HasColumnName("gender").HasMaxLength(10);
                entity.Property(e => e.Nationality).HasColumnName("nationality").HasMaxLength(50).HasDefaultValue("Việt Nam");
                entity.Property(e => e.PlaceOfOrigin).HasColumnName("place_of_origin").HasMaxLength(300);
                entity.Property(e => e.PlaceOfResidence).HasColumnName("place_of_residence").HasMaxLength(300);
                entity.Property(e => e.IssueDate).HasColumnName("issue_date");
                entity.Property(e => e.ExpireDate).HasColumnName("expire_date");
                entity.Property(e => e.IssuePlace).HasColumnName("issue_place").HasMaxLength(300);
                entity.Property(e => e.FrontImageUrl).HasColumnName("front_image_url");
                entity.Property(e => e.BackImageUrl).HasColumnName("back_image_url");

                entity.Property(e => e.VerificationStatus)
                    .HasColumnName("verification_status")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDefaultValue("UNVERIFIED");

                entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");
                entity.Property(e => e.VerifiedBy).HasColumnName("verified_by").HasMaxLength(50);

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

                entity.Property(e => e.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

                entity.HasIndex(e => e.UserId).IsUnique().HasDatabaseName("UQ_user_person_ids_user_id");
                entity.HasIndex(e => e.IdNumber).IsUnique().HasDatabaseName("UQ_user_person_ids_id_number");
                entity.HasIndex(e => e.VerificationStatus).HasDatabaseName("IX_user_person_ids_verification_status");

                entity.HasOne(e => e.User)
                    .WithOne(u => u.UserPersonId)
                    .HasForeignKey<UserPersonId>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
