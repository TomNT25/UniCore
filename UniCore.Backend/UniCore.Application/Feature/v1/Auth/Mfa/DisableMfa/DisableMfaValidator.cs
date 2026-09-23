using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.Mfa.DisableMfa
{
    public class DisableMfaValidator : AbstractValidator<DisableMfaRequestDTO>
    {
        public DisableMfaValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password confirmation is required.");
        }
    }
}
