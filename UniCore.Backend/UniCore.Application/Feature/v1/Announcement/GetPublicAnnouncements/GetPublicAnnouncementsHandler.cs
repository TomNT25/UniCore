using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;

namespace UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements
{
    public class GetPublicAnnouncementsHandler : IRequestHandler<GetPublicAnnouncementsRequestDTO, GetPublicAnnouncementsResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public GetPublicAnnouncementsHandler(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<GetPublicAnnouncementsResponseDTO> HandleAsync(
            GetPublicAnnouncementsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;
            var items = await _announcementRepository.GetPublishedPublicAsync(cancellationToken);

            return new GetPublicAnnouncementsResponseDTO
            {
                Items = items
                    .Select(a => AdminAnnouncementMapping.ToAdminItem(a, utcNow, includeContent: false))
                    .ToList()
            };
        }
    }
}
