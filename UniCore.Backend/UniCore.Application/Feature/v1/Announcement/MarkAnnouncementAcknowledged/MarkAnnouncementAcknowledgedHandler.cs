using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.MarkAnnouncementAcknowledged
{
    public class MarkAnnouncementAcknowledgedHandler : IRequestHandler<MarkAnnouncementAcknowledgedRequestDTO, MarkAnnouncementAcknowledgedResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly ILogger<MarkAnnouncementAcknowledgedHandler> _logger;

        public MarkAnnouncementAcknowledgedHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            ILogger<MarkAnnouncementAcknowledgedHandler> logger)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _logger = logger;
        }

        public async Task<MarkAnnouncementAcknowledgedResponseDTO> HandleAsync(
            MarkAnnouncementAcknowledgedRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                "Marking announcement {AnnouncementId} as acknowledged by student {StudentId}",
                request.AnnouncementId, request.StudentId);

            // Verify announcement exists
            var announcement = await _announcementRepository.GetByIdAsync(request.AnnouncementId, cancellationToken);
            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement {request.AnnouncementId} not found");
            }

            // Mark as acknowledged (this also marks as viewed if not already)
            var acknowledgedAt = await _announcementStudentRepository.MarkAsAcknowledgedAsync(
                request.AnnouncementId,
                request.StudentId,
                cancellationToken);

            _logger.LogInformation(
                "Announcement {AnnouncementId} marked as acknowledged by student {StudentId} at {AcknowledgedAt}",
                request.AnnouncementId, request.StudentId, acknowledgedAt);

            return new MarkAnnouncementAcknowledgedResponseDTO
            {
                Success = true,
                AnnouncementId = request.AnnouncementId,
                AcknowledgedAt = acknowledgedAt
            };
        }
    }
}
