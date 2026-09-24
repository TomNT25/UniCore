using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.EnableFaceMfa
{
    public class EnableFaceMfaValidator : AbstractValidator<EnableFaceMfaRequestDTO>
    {
        public EnableFaceMfaValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.UserIdRequired);
        }
    }
}
