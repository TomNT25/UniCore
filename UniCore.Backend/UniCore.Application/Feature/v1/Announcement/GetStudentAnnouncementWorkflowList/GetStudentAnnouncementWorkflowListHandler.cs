using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement.Workflow;

namespace UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncementWorkflowList
{
    public class GetStudentAnnouncementWorkflowListRequest : IRequest<StudentAnnouncementWorkflowListResponse>
    {
        public string StudentId { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
    }

    public class GetStudentAnnouncementWorkflowListHandler
        : IRequestHandler<GetStudentAnnouncementWorkflowListRequest, StudentAnnouncementWorkflowListResponse>
    {
        private const int WorkflowListCap = 500;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;

        public GetStudentAnnouncementWorkflowListHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
        }

        public async Task<StudentAnnouncementWorkflowListResponse> HandleAsync(
            GetStudentAnnouncementWorkflowListRequest request,
            CancellationToken cancellationToken)
        {
            var timeStatus = request.Status.Trim().ToLowerInvariant();
            if (timeStatus is not ("active" or "expired"))
            {
                throw new ArgumentException("Status must be active or expired.");
            }

            var (announcements, _) = await _announcementRepository.GetAnnouncementsForStudentAsync(
                request.StudentId,
                typeFilter: null,
                timeStatus: timeStatus,
                page: 1,
                pageSize: WorkflowListCap,
                cancellationToken);

            var ids = announcements.Select(a => a.Id).ToList();
            var links = await _announcementStudentRepository.GetReadStatusesAsync(
                request.StudentId,
                ids,
                cancellationToken);

            var utcNow = DateTime.UtcNow;
            var items = announcements
                .Select(a =>
                {
                    links.TryGetValue(a.Id, out var link);
                    return StudentAnnouncementWorkflowMapping.ToWorkflowItem(a, link, utcNow, includeContent: false);
                })
                .ToList();

            return new StudentAnnouncementWorkflowListResponse { Items = items };
        }
    }
}
