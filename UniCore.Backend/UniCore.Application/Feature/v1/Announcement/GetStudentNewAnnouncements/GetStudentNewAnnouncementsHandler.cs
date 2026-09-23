using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement.Workflow;

namespace UniCore.Application.Feature.v1.Announcement.GetStudentNewAnnouncements
{
    public class GetStudentNewAnnouncementsRequest : IRequest<StudentAnnouncementWorkflowListResponse>
    {
        public string StudentId { get; set; } = string.Empty;
    }

    public class GetStudentNewAnnouncementsHandler
        : IRequestHandler<GetStudentNewAnnouncementsRequest, StudentAnnouncementWorkflowListResponse>
    {
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;

        public GetStudentNewAnnouncementsHandler(IAnnouncementStudentRepository announcementStudentRepository)
        {
            _announcementStudentRepository = announcementStudentRepository;
        }

        public async Task<StudentAnnouncementWorkflowListResponse> HandleAsync(
            GetStudentNewAnnouncementsRequest request,
            CancellationToken cancellationToken)
        {
            var unsent = await _announcementStudentRepository.GetUnsentWithAnnouncementAsync(
                request.StudentId,
                cancellationToken);

            if (unsent.Count == 0)
            {
                return new StudentAnnouncementWorkflowListResponse();
            }

            var utcNow = DateTime.UtcNow;
            var items = unsent
                .Select(row => StudentAnnouncementWorkflowMapping.ToWorkflowItem(
                    row.Announcement,
                    row,
                    utcNow,
                    includeContent: false))
                .ToList();

            await _announcementStudentRepository.MarkAsSentAsync(
                unsent.Select(s => s.Id),
                cancellationToken);

            return new StudentAnnouncementWorkflowListResponse { Items = items };
        }
    }
}
