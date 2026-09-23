using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.GetCourseById
{
    public class GetCourseByIdRequestDTO : IRequest<GetCourseByIdResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
