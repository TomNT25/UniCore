using UniCore.Application.Feature.v1.Announcement.CreateAnnouncement;
using UniCore.Application.Feature.v1.Announcement.DeleteAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetDeliveryReport;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements;
using UniCore.Application.Feature.v1.Announcement.PreviewRecipients;
using UniCore.Application.Feature.v1.Announcement.UpdateAnnouncement;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IAnnouncementService
    {
        Task<GetAllAnnouncementResponseDTO> GetAllAsync(GetAllAnnouncementRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetAnnouncementByIdResponseDTO> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CreateAnnouncementResponseDTO> CreateAsync(CreateAnnouncementRequestDTO request, CancellationToken cancellationToken = default);
        Task<UpdateAnnouncementResponseDTO> UpdateAsync(UpdateAnnouncementRequestDTO request, CancellationToken cancellationToken = default);
        Task<DeleteAnnouncementResponseDTO> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<PreviewRecipientsResponseDTO> PreviewRecipientsAsync(PreviewRecipientsRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetDeliveryReportResponseDTO> GetDeliveryReportAsync(string id, CancellationToken cancellationToken = default);
        Task<GetPublicAnnouncementsResponseDTO> GetPublicListAsync(CancellationToken cancellationToken = default);
        Task<GetPublicAnnouncementByIdResponseDTO> GetPublicByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
