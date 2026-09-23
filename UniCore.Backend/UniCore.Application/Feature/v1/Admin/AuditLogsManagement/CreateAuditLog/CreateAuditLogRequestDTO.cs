using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.CreateAuditLog
{
    public class CreateAuditLogRequestDTO : IRequest<CreateAuditLogResponseDTO>
    {
        public string LogLevel { get; set; } = string.Empty;
        public string? Thread { get; set; }
        public string? Logger { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public string? MachineName { get; set; }
        public string? TraceId { get; set; }
    }
}
