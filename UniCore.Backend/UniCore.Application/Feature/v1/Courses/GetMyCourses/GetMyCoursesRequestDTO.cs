using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
    public class GetMyCoursesRequestDTO : PageNumberPaginationRequest, IRequest<GetMyCoursesResponseDTO>
    {
        public string UserID {  get; set; }
    }
}
