
using UniCore.Application.Feature.v1.Courses.GetAllCourses;

namespace UniCore.Application.Contract.Service.v1
{
    public interface ICourseService
    {
        Task<GetAllCoursesResponseDTO> GetAllCoursesByDepartment(GetAllCoursesRequestDTO request, CancellationToken cancellationToken);
    }
}
