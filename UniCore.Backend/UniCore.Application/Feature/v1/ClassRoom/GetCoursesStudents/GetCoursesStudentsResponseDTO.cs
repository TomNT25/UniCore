
namespace UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents
{
    public class GetCoursesStudentsResponseDTO
    {
        public string CourseStartDate { get; set; } = "";
        public string CourseEndDate { get; set; } = "";
        public string CourseName { get; set; } = "";
        public string CourseDescription { get; set; } = "";
        public string CourseStatus { get; set; } = "";
        public decimal CourseFinalScore { get; set; }
    }
}
