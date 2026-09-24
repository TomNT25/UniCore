using UniCore.Application.Feature.v1.Announcement.Targets;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IAnnouncementTargetSearchService
    {
        Task<SearchCourseTargetsResponseDTO> SearchCoursesAsync(
            SearchCourseTargetsRequestDTO request,
            CancellationToken cancellationToken = default);

        Task<SearchClassTargetsResponseDTO> SearchClassesAsync(
            SearchClassTargetsRequestDTO request,
            CancellationToken cancellationToken = default);

        Task<SearchDepartmentTargetsResponseDTO> SearchDepartmentsAsync(
            SearchDepartmentTargetsRequestDTO request,
            CancellationToken cancellationToken = default);

        Task<SearchStudentTargetsResponseDTO> SearchStudentsAsync(
            SearchStudentTargetsRequestDTO request,
            CancellationToken cancellationToken = default);
    }
}
