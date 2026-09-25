using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Courses.GetMyCourses.InternalDTOs;


namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.ListDetails
{
    public class GetMyCoursesResponseDTO : PageNumberPaginationResponse<GetMyCoursesDTO>
    {
        public decimal Score { get; set; }
    }
}
