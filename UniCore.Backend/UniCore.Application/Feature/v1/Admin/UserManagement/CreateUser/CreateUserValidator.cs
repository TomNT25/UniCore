using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequestDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role ID is required.");
        }
    }
}
