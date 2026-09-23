using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.ForgotPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequestDTO>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP code is required")
                .Length(6).WithMessage("OTP code must be 6 digits");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters");
        }
    }
}
