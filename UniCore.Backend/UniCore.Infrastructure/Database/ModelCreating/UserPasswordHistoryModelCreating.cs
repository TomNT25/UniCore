using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserPasswordHistoryModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPasswordHistory>(entity =>
            {
                entity.ToTable("user_password_histories");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => new { e.UserId, e.CreatedAt })
                    .HasDatabaseName("IX_user_password_histories_user_id_date");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserPasswordHistories)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
