using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.GetAllCourses
{
    public class GetAllCoursesRequestDTO : PageNumberPaginationRequest, IRequest<GetAllCoursesResponseDTO>
    {
    }
}
