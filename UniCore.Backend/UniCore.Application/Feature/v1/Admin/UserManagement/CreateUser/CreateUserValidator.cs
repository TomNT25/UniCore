using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Matches(@"^[a-zA-Z0-9._-]+$").WithMessage("Username can only contain alphanumeric characters, dots, underscores, or hyphens.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role ID is required.")
                .MaximumLength(50).WithMessage("Role ID cannot exceed 50 characters.");

            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.")
                    .Matches(@"^\+?[0-9\s\-()]{7,20}$").WithMessage("Invalid phone number format.");
            });
        }
    }
}
