using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class VerifyOtpValidator : AbstractValidator<VerifyOtpRequestDTO>
    {
        public VerifyOtpValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP code is required")
                .Length(6).WithMessage("OTP code must be 6 digits");
        }
    }
}
