
namespace UniCore.Application.Feature.v1.Courses.GetAllCourses
{
    public class GetAllCoursesDTO
    {
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
