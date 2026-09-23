using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Announcement.CreateAnnouncement;
using UniCore.Application.Feature.v1.Announcement.DeleteAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetDeliveryReport;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements;
using UniCore.Application.Feature.v1.Announcement.PreviewRecipients;
using UniCore.Application.Feature.v1.Announcement.UpdateAnnouncement;

namespace UniCore.Application.Service.v1
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly GetAllAnnouncementHandler _getAllHandler;
        private readonly GetAnnouncementByIdHandler _getByIdHandler;
        private readonly CreateAnnouncementHandler _createHandler;
        private readonly UpdateAnnouncementHandler _updateHandler;
        private readonly DeleteAnnouncementHandler _deleteHandler;
        private readonly PreviewRecipientsHandler _previewHandler;
        private readonly GetDeliveryReportHandler _deliveryReportHandler;
        private readonly GetPublicAnnouncementsHandler _getPublicListHandler;
        private readonly GetPublicAnnouncementByIdHandler _getPublicByIdHandler;

        public AnnouncementService(
            GetAllAnnouncementHandler getAllHandler,
            GetAnnouncementByIdHandler getByIdHandler,
            CreateAnnouncementHandler createHandler,
            UpdateAnnouncementHandler updateHandler,
            DeleteAnnouncementHandler deleteHandler,
            PreviewRecipientsHandler previewHandler,
            GetDeliveryReportHandler deliveryReportHandler,
            GetPublicAnnouncementsHandler getPublicListHandler,
            GetPublicAnnouncementByIdHandler getPublicByIdHandler)
        {
            _getAllHandler = getAllHandler;
            _getByIdHandler = getByIdHandler;
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _previewHandler = previewHandler;
            _deliveryReportHandler = deliveryReportHandler;
            _getPublicListHandler = getPublicListHandler;
            _getPublicByIdHandler = getPublicByIdHandler;
        }

        public Task<GetAllAnnouncementResponseDTO> GetAllAsync(GetAllAnnouncementRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllHandler.HandleAsync(request, cancellationToken);

        public Task<GetAnnouncementByIdResponseDTO> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getByIdHandler.HandleAsync(new GetAnnouncementByIdRequestDTO { Id = id }, cancellationToken);

        public Task<CreateAnnouncementResponseDTO> CreateAsync(CreateAnnouncementRequestDTO request, CancellationToken cancellationToken = default)
            => _createHandler.HandleAsync(request, cancellationToken);

        public Task<UpdateAnnouncementResponseDTO> UpdateAsync(UpdateAnnouncementRequestDTO request, CancellationToken cancellationToken = default)
            => _updateHandler.HandleAsync(request, cancellationToken);

        public Task<DeleteAnnouncementResponseDTO> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => _deleteHandler.HandleAsync(new DeleteAnnouncementRequestDTO { Id = id }, cancellationToken);

        public Task<PreviewRecipientsResponseDTO> PreviewRecipientsAsync(PreviewRecipientsRequestDTO request, CancellationToken cancellationToken = default)
            => _previewHandler.HandleAsync(request, cancellationToken);

        public Task<GetDeliveryReportResponseDTO> GetDeliveryReportAsync(string id, CancellationToken cancellationToken = default)
            => _deliveryReportHandler.HandleAsync(new GetDeliveryReportRequestDTO { Id = id }, cancellationToken);

        public Task<GetPublicAnnouncementsResponseDTO> GetPublicListAsync(CancellationToken cancellationToken = default)
            => _getPublicListHandler.HandleAsync(new GetPublicAnnouncementsRequestDTO(), cancellationToken);

        public Task<GetPublicAnnouncementByIdResponseDTO> GetPublicByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getPublicByIdHandler.HandleAsync(new GetPublicAnnouncementByIdRequestDTO { Id = id }, cancellationToken);
    }
}
