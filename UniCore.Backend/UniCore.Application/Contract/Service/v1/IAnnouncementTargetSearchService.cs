using UniCore.Application.Feature.v1.Announcement.Targets;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IAnnouncementTargetSearchService
    {
        Task<AnnouncementTargetSearchResponseDto<CourseTargetItemDto>> SearchCoursesAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default);

        Task<AnnouncementTargetSearchResponseDto<ClassTargetItemDto>> SearchClassesAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default);

        Task<AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>> SearchDepartmentsAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default);

        Task<AnnouncementTargetSearchResponseDto<StudentTargetItemDto>> SearchStudentsAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default);
    }
}
