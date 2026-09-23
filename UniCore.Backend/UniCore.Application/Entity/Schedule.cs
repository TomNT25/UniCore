namespace UniCore.Application.Entity
{
    public class Schedule
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string StudentCourseId { get; set; } = string.Empty;
        public string TimeSlot { get; set; } = string.Empty;
        public DateTime DayOccur { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual CourseStudent CourseStudent { get; set; } = null!;
    }
}
