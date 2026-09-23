namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs
{
    public class GetAllAuditLogsDTO
    {
        public string Id { get; set; } = string.Empty;
        public DateTime LogDate { get; set; }
        public string? Thread { get; set; }
        public string LogLevel { get; set; } = string.Empty;
        public string? Logger { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public string? MachineName { get; set; }
        public string? TraceId { get; set; }
    }
}
