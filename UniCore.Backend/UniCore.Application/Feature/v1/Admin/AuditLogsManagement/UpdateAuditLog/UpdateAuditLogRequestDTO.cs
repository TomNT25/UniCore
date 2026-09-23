using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.UpdateAuditLog
{
    public class UpdateAuditLogRequestDTO : IRequest<UpdateAuditLogResponseDTO>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        public string LogLevel { get; set; } = string.Empty;
        public string? Thread { get; set; }
        public string? Logger { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public string? MachineName { get; set; }
        public string? TraceId { get; set; }
    }
}
