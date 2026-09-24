using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
    public class GetStudentCoursesDTO
    {
        public string Id { get; set; }
        public string Code { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Weight { get; set; }

        // Nullable!
        public decimal? FinalScore { get; set; }

        public string Status { get; set; } = null!;
    }
}
