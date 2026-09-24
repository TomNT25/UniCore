using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.DisableFaceMfa
{
    public class DisableFaceMfaValidator : AbstractValidator<DisableFaceMfaRequestDTO>
    {
        public DisableFaceMfaValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.UserIdRequired);
        }
    }
}
