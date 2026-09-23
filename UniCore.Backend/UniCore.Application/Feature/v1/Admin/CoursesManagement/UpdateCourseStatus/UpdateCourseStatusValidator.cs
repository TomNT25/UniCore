using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourseStatus
{
    public class UpdateCourseStatusValidator : AbstractValidator<UpdateCourseStatusRequestDTO>
    {
        public UpdateCourseStatusValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course ID is required.");
        }
    }
}
