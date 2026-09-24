using System;
using System.Collections.Generic;
using System.Text;
using UniCore.Application.Feature.v1.Courses.GetMyCourses;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails
{
    public class GetCourseDetailsResponseDTO
    {
        public GetCourseDTO Courses { get; set; }

        public GetStudentCoursesDTO StudentCourses { get; set; }
    }
}
