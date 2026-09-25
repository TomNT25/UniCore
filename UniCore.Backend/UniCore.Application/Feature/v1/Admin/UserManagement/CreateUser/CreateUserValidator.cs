using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.StudentCode))
                .WithMessage("student_code must not be empty");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email must not be empty")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
        }
    }
}
