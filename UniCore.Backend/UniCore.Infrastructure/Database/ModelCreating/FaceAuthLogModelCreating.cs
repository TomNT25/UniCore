using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class FaceAuthLogModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FaceAuthLog>(entity =>
            {
                entity.ToTable("face_auth_logs");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.RequestId)
                    .HasColumnName("request_id")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50);

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ErrorCode)
                    .HasColumnName("error_code")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelVersion)
                    .HasColumnName("model_version")
                    .HasMaxLength(50);

                entity.Property(e => e.Similarity)
                    .HasColumnName("similarity");

                entity.Property(e => e.LatencyMs)
                    .HasColumnName("latency_ms");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasMaxLength(50);

                entity.Property(e => e.UserAgent)
                    .HasColumnName("user_agent")
                    .HasMaxLength(500);

                entity.Property(e => e.Metadata)
                    .HasColumnName("metadata")
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");

                // Index for querying logs by user
                entity.HasIndex(e => e.UserId);

                // Index for querying logs by action
                entity.HasIndex(e => e.Action);

                // Index for querying logs by time
                entity.HasIndex(e => e.CreatedAt);
            });
        }
    }
}
