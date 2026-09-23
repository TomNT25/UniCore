using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Courses.GetAllCourses;

namespace UniCore.Application.Service.v1
{
    public class CourseService : ICourseService
    {
        private readonly GetAllCoursesDepartmentHandler _getAllCoursesDepartmentHandler;
        public CourseService(GetAllCoursesDepartmentHandler getAllCoursesDepartmentHandler) 
        {
            _getAllCoursesDepartmentHandler = getAllCoursesDepartmentHandler;
        }
        public async Task<GetAllCoursesResponseDTO> GetAllCoursesByDepartment(GetAllCoursesRequestDTO request, CancellationToken cancellationToken)
            => await _getAllCoursesDepartmentHandler.HandleAsync(request, cancellationToken);
    }
}
