using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.DeleteAuditLog
{
    public class DeleteAuditLogRequestDTO : IRequest<DeleteAuditLogResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
