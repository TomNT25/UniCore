using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.Mfa.EnableMfa
{
    public class EnableMfaValidator : AbstractValidator<EnableMfaRequestDTO>
    {
        public EnableMfaValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Code).NotEmpty().WithMessage("MFA Code is required.");
        }
    }
}
