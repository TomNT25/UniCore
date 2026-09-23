using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.DeleteCourse
{
    public class DeleteCourseRequestDTO : IRequest<DeleteCourseResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
