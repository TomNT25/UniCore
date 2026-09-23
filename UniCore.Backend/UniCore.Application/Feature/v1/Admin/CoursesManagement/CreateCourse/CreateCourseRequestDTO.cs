using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.CreateCourse
{
    public class CreateCourseRequestDTO : IRequest<CreateCourseResponseDTO>
    {
        public string? Code { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DepartmentId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
