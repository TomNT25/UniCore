
namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
    public class GetDepartmentsDTO
    {
        public string Id { get; set; } = null!; 
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
    }
}
