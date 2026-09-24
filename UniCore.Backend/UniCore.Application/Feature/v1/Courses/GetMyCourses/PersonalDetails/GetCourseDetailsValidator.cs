using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails
{
    public class GetCourseDetailsValidator : AbstractValidator<GetCourseDetailsRequestDTO>
    {
        public GetCourseDetailsValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
            RuleFor(x => x.CourseID)
                .NotEmpty()
                .WithMessage("Please specify a Course ID");
        }
    }
}
