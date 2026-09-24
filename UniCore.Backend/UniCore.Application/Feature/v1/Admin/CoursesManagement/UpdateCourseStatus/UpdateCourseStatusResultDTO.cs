namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourseStatus
{
    public class UpdateCourseStatusResultDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
