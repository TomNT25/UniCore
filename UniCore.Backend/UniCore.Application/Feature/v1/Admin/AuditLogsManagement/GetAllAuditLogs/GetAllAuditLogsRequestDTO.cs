using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs
{
    public class GetAllAuditLogsRequestDTO : PageNumberPaginationRequest, IRequest<GetAllAuditLogsResponseDTO>
    {
        public string? LogLevel { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TraceId { get; set; }
    }
}
