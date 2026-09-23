using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncements
{
    public class GetStudentAnnouncementsHandler : IRequestHandler<GetStudentAnnouncementsRequestDTO, GetStudentAnnouncementsResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly ILogger<GetStudentAnnouncementsHandler> _logger;

        public GetStudentAnnouncementsHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            ILogger<GetStudentAnnouncementsHandler> logger)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _logger = logger;
        }

        public async Task<GetStudentAnnouncementsResponseDTO> HandleAsync(
            GetStudentAnnouncementsRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Getting announcements for student {StudentId}", request.StudentId);

            // Get announcements targeted to this student
            var announcements = await _announcementRepository.GetAnnouncementsForStudentAsync(
                request.StudentId,
                request.Type,
                timeStatus: "active",
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            // Get read status for these announcements
            var announcementIds = announcements.Items.Select(a => a.Id).ToList();
            var readStatuses = await _announcementStudentRepository.GetReadStatusesAsync(
                request.StudentId,
                announcementIds,
                cancellationToken);

            var now = DateTime.UtcNow;
            var items = announcements.Items.Select(a =>
            {
                readStatuses.TryGetValue(a.Id, out var status);
                return new StudentAnnouncementItemDTO
                {
                    Id = a.Id,
                    Code = a.Code ?? string.Empty,
                    Title = a.Title,
                    Summary = a.Content?.Length > 200 ? a.Content[..200] + "..." : a.Content,
                    Type = a.Type?.ToUpperInvariant() ?? AnnouncementConstants.Type.Normal,
                    ScopeType = a.ScopeType?.ToUpperInvariant() ?? AnnouncementConstants.Scope.Public,
                    Status = AnnouncementLifecycle.ComputeStatus(a.PublishDate, a.ExpiredDate, now),
                    PublishDate = AdminAnnouncementMapping.AsUtc(a.PublishDate),
                    ExpiredDate = AdminAnnouncementMapping.AsUtc(a.ExpiredDate),
                    CreatedBy = a.CreatedBy,
                    CreatedAt = a.CreatedAt,
                    IsRead = status?.ViewedAt != null,
                    ViewedAt = status?.ViewedAt,
                    IsAcknowledged = status?.AcknowledgedAt != null,
                    AcknowledgedAt = status?.AcknowledgedAt
                };
            }).ToList();

            // Apply read filter if specified
            if (request.IsRead.HasValue)
            {
                items = items.Where(i => i.IsRead == request.IsRead.Value).ToList();
            }

            // Count unread
            var unreadCount = items.Count(i => !i.IsRead);

            return new GetStudentAnnouncementsResponseDTO
            {
                Items = items,
                TotalRecords = announcements.TotalCount,
                UnreadCount = unreadCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(announcements.TotalCount / (double)request.PageSize)
            };
        }
    }
}
