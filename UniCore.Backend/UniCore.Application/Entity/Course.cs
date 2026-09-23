namespace UniCore.Application.Entity
{
    public class Course
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DepartmentId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
        public virtual ICollection<ClassCourse> ClassCourses { get; set; } = new List<ClassCourse>();
    }
}
