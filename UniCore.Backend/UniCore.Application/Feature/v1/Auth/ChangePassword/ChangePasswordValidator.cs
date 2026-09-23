using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequestDTO>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required.");
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("New password must be at least 6 characters.");
        }
    }
}
