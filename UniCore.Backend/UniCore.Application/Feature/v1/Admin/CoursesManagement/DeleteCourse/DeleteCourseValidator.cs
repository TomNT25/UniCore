using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.DeleteCourse
{
    public class DeleteCourseValidator : AbstractValidator<DeleteCourseRequestDTO>
    {
        public DeleteCourseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course ID is required.");
        }
    }
}
