using FluentValidation;

namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
    public class GetMyCoursesValidator : AbstractValidator<GetMyCoursesRequestDTO>
    {
        public GetMyCoursesValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
        }  
    }
}
