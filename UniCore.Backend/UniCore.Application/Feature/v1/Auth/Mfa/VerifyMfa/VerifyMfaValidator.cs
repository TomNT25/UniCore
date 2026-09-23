using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.Mfa.VerifyMfa
{
    public class VerifyMfaValidator : AbstractValidator<VerifyMfaRequestDTO>
    {
        public VerifyMfaValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Code).NotEmpty().WithMessage("MFA Code is required.");
        }
    }
}
