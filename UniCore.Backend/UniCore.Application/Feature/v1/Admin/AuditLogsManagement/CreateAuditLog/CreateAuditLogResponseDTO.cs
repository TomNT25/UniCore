using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.CreateAuditLog
{
    public class CreateAuditLogResponseDTO
    {
        public AppLogDTO Log { get; set; } = null!;
    }
}
