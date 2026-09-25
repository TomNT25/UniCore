using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.InternalDTOs
{
    public class GetCourseDetailsScheduleDTO
    {
        public string? Code { get; set; }
        public string StudentCourseId { get; set; } = string.Empty;
        public string TimeSlot { get; set; } = string.Empty;
        public DateTime DayOccur { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
