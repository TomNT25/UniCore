using FluentValidation;

namespace UniCore.Application.Feature.v1.Auth.Mfa.SetupMfa
{
    public class SetupMfaValidator : AbstractValidator<SetupMfaRequestDTO>
    {
        public SetupMfaValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
