using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourse
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseRequestDTO>
    {
        public UpdateCourseValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Course name is required.")
                .MaximumLength(100).WithMessage("Course name must not exceed 100 characters.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Course type is required.")
                .MaximumLength(20).WithMessage("Course type must not exceed 20 characters.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.")
                .MaximumLength(50).WithMessage("Department ID must not exceed 50 characters.");

            When(x => !string.IsNullOrWhiteSpace(x.Code), () =>
            {
                RuleFor(x => x.Code)
                    .MaximumLength(50).WithMessage("Course code must not exceed 50 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
            });
        }
    }
}
