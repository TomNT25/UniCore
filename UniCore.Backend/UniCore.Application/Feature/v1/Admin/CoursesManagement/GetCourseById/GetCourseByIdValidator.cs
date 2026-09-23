using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.GetCourseById
{
    public class GetCourseByIdValidator : AbstractValidator<GetCourseByIdRequestDTO>
    {
        public GetCourseByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course ID is required.");
        }
    }
}
