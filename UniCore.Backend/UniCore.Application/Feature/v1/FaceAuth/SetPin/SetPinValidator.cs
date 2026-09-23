using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.SetPin
{
    public class SetPinValidator : AbstractValidator<SetPinRequestDTO>
    {
        public SetPinValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.UserIdRequired);

            RuleFor(x => x.Pin)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.PinRequired)
                .Length(FaceAuthConstants.Pin.Length)
                .WithMessage(MessageConstants.FaceAuth.PinInvalidLength)
                .Matches(@"^\d{6}$")
                .WithMessage(MessageConstants.FaceAuth.PinMustBeDigits);

            RuleFor(x => x.ConfirmPin)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.ConfirmPinRequired)
                .Equal(x => x.Pin)
                .WithMessage(MessageConstants.FaceAuth.PinMismatch);
        }
    }
}
