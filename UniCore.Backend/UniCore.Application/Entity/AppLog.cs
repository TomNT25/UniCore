namespace UniCore.Application.Entity
{
    public class AppLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime LogDate { get; set; } = DateTime.UtcNow;
        public string? Thread { get; set; }
        public string LogLevel { get; set; } = string.Empty;
        public string? Logger { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public string? MachineName { get; set; }
        public string? TraceId { get; set; }
    }
}
