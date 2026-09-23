namespace UniCore.Application.Entity
{
    public class CourseStudent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string CourseId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Weight { get; set; } = 1;
        public decimal? FinalScore { get; set; }
        public string? Status { get; set; } = "ENROLLED";
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
