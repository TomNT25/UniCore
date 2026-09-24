namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.CreateCourse
{
    public class CreateCourseResultDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DepartmentId { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
