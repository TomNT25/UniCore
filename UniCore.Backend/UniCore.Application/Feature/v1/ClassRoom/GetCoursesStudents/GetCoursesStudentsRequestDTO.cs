using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents
{
    public class GetCoursesStudentsRequestDTO : IRequest<GetCoursesStudentsResponseDTO>
    {
        public string UserID { get; set; }
        public IEnumerable<string> CourseIDs { get; set; }
    }
}
