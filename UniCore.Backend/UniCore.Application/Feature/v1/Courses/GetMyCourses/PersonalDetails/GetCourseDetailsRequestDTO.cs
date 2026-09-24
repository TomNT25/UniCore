using System;
using System.Collections.Generic;
using System.Text;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails
{
    public class GetCourseDetailsRequestDTO : IRequest<GetCourseDetailsResponseDTO>
    {
        public string UserID { get; set; }
        public string CourseID { get; set; }
    }
}
