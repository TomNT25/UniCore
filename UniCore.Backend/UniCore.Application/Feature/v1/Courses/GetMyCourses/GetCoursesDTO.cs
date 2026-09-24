namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
    public class GetCourseDTO
    {
        public string Id { get; set; } = "";
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;

        public GetDepartmentsDTO Department { get; set; } = null!;
    }
}
