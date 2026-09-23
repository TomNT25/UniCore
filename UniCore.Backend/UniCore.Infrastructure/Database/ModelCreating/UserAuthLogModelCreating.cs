using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class UserAuthLogModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuthLog>(entity =>
            {
                entity.ToTable("user_auth_logs");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.UserId).HasColumnName("user_id").HasMaxLength(50);
                entity.Property(e => e.EventType).HasColumnName("event_type").IsRequired().HasMaxLength(50);
                entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(45);
                entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => new { e.UserId, e.CreatedAt })
                    .HasDatabaseName("IX_user_auth_logs_user_id_date");

                entity.HasIndex(e => new { e.EventType, e.CreatedAt })
                    .HasDatabaseName("IX_user_auth_logs_event_type");

                entity.HasIndex(e => e.CreatedAt)
                    .HasDatabaseName("IX_user_auth_logs_created_at");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserAuthLogs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
