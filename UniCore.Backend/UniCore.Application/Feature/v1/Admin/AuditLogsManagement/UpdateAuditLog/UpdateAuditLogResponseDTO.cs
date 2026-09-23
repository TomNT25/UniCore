using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.UpdateAuditLog
{
    public class UpdateAuditLogResponseDTO
    {
        public AppLogDTO Log { get; set; } = null!;
    }
}
