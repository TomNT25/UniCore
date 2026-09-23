using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.Login
{
    public class LoginValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        }
    }
}
