using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.MarkAnnouncementViewed
{
    public class MarkAnnouncementViewedHandler : IRequestHandler<MarkAnnouncementViewedRequestDTO, MarkAnnouncementViewedResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly ILogger<MarkAnnouncementViewedHandler> _logger;

        public MarkAnnouncementViewedHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            ILogger<MarkAnnouncementViewedHandler> logger)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _logger = logger;
        }

        public async Task<MarkAnnouncementViewedResponseDTO> HandleAsync(
            MarkAnnouncementViewedRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                "Marking announcement {AnnouncementId} as viewed by student {StudentId}",
                request.AnnouncementId, request.StudentId);

            // Verify announcement exists and is active
            var announcement = await _announcementRepository.GetByIdAsync(request.AnnouncementId, cancellationToken);
            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement {request.AnnouncementId} not found");
            }

            // Mark as viewed
            var viewedAt = await _announcementStudentRepository.MarkAsViewedAsync(
                request.AnnouncementId,
                request.StudentId,
                cancellationToken);

            _logger.LogInformation(
                "Announcement {AnnouncementId} marked as viewed by student {StudentId} at {ViewedAt}",
                request.AnnouncementId, request.StudentId, viewedAt);

            return new MarkAnnouncementViewedResponseDTO
            {
                Success = true,
                AnnouncementId = request.AnnouncementId,
                ViewedAt = viewedAt
            };
        }
    }
}
