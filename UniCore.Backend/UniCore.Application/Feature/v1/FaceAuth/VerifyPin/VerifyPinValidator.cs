using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyPin
{
    public class VerifyPinValidator : AbstractValidator<VerifyPinRequestDTO>
    {
        public VerifyPinValidator()
        {
            RuleFor(x => x.ChallengeToken)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.InvalidChallenge);

            RuleFor(x => x.Pin)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.PinRequired)
                .Length(FaceAuthConstants.Pin.Length)
                .WithMessage(MessageConstants.FaceAuth.PinInvalidLength)
                .Matches(@"^\d{6}$")
                .WithMessage(MessageConstants.FaceAuth.PinMustBeDigits);
        }
    }
}
