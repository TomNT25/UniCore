using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.GetDeliveryReport
{
    public class GetDeliveryReportRequestDTO : IRequest<GetDeliveryReportResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
