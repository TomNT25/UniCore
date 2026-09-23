namespace UniCore.Application.Entity
{
    public class ClassCourse
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string SchoolClassId { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual SchoolClass SchoolClass { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;

        //public virtual ICollection<Course> Courses { get; set; } = new List<Course>();


    }
}
