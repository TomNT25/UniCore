using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDTO>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.")
                .MaximumLength(50).WithMessage("User ID cannot exceed 50 characters.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Matches(@"^[a-zA-Z0-9._-]+$").WithMessage("Username can only contain alphanumeric characters, dots, underscores, or hyphens.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role ID is required.")
                .MaximumLength(50).WithMessage("Role ID cannot exceed 50 characters.");
        }
    }
}
