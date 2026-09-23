using Microsoft.EntityFrameworkCore;
using UniCore.Application.Entity;

namespace UniCore.Infrastructure.Database.ModelCreating
{
    public static class AppLogModelCreating
    {
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppLog>(entity =>
            {
                entity.ToTable("app_logs");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("dbo.fn_GenerateUUIDv7()");

                entity.Property(e => e.LogDate).HasColumnName("log_date").IsRequired();
                entity.Property(e => e.Thread).HasColumnName("thread").HasMaxLength(255);
                entity.Property(e => e.LogLevel).HasColumnName("log_level").IsRequired().HasMaxLength(50);
                entity.Property(e => e.Logger).HasColumnName("logger").HasMaxLength(255);
                entity.Property(e => e.Message).HasColumnName("message");
                entity.Property(e => e.Exception).HasColumnName("exception");
                entity.Property(e => e.MachineName).HasColumnName("machine_name").HasMaxLength(255);
                entity.Property(e => e.TraceId).HasColumnName("trace_id").HasMaxLength(255);

                entity.HasIndex(e => e.LogDate)
                    .HasDatabaseName("IX_app_logs_log_date");

                entity.HasIndex(e => new { e.LogLevel, e.LogDate })
                    .HasDatabaseName("IX_app_logs_level_date");

                entity.HasIndex(e => e.TraceId)
                    .HasDatabaseName("IX_app_logs_trace_id")
                    .HasFilter("[trace_id] IS NOT NULL");
            });
        }
    }
}
