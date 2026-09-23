using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement.Workflow;

namespace UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncementById
{
    public class GetStudentAnnouncementByIdRequest : IRequest<StudentAnnouncementWorkflowItemDto>
    {
        public string StudentId { get; set; } = string.Empty;
        public string AnnouncementId { get; set; } = string.Empty;
    }

    public class GetStudentAnnouncementByIdHandler
        : IRequestHandler<GetStudentAnnouncementByIdRequest, StudentAnnouncementWorkflowItemDto>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;

        public GetStudentAnnouncementByIdHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
        }

        public async Task<StudentAnnouncementWorkflowItemDto> HandleAsync(
            GetStudentAnnouncementByIdRequest request,
            CancellationToken cancellationToken)
        {
            var announcement = await _announcementRepository.GetAnnouncementVisibleToStudentAsync(
                request.StudentId,
                request.AnnouncementId,
                cancellationToken);

            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement {request.AnnouncementId} not found or not accessible.");
            }

            await _announcementStudentRepository.MarkAsViewedAsync(
                request.AnnouncementId,
                request.StudentId,
                cancellationToken);

            var link = await _announcementStudentRepository.GetLinkAsync(
                request.StudentId,
                request.AnnouncementId,
                cancellationToken);

            return StudentAnnouncementWorkflowMapping.ToWorkflowItem(
                announcement,
                link,
                DateTime.UtcNow,
                includeContent: true);
        }
    }
}
