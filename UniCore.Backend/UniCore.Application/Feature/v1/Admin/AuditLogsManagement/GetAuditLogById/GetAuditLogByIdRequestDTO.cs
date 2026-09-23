using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAuditLogById
{
    public class GetAuditLogByIdRequestDTO : IRequest<GetAuditLogByIdResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
