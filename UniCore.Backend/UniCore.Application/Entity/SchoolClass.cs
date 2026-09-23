namespace UniCore.Application.Entity
{
    public class SchoolClass
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<User> Students { get; set; } = new List<User>();
        public virtual ICollection<ClassCourse> ClassCourses { get; set; } = new List<ClassCourse>();
    }
}
