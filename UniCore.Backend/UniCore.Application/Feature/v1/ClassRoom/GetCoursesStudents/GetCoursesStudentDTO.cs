using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents
{
    public class GetCoursesStudentDTO
    {
        public string CourseCode { get; set; } = "";
        public string CourseStartDate { get; set; } = "";
        public string CourseEndDate { get; set; } = "";
        public string CourseStatus { get; set; } = "";
        public decimal CourseFinalScore { get; set; }
    }
}
