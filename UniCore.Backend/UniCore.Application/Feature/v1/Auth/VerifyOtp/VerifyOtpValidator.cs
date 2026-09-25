using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class VerifyOtpValidator : AbstractValidator<VerifyOtpRequestDTO>
    {
        public VerifyOtpValidator()
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Email) || !string.IsNullOrWhiteSpace(x.Username))
                .WithMessage("Email or Username is required");

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email!)
                    .EmailAddress().WithMessage("Invalid email format");
            });

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP code is required")
                .Length(6).WithMessage("OTP code must be 6 digits");
        }
    }
}
