using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyOtp
{
    public class VerifyOtpValidator : AbstractValidator<VerifyOtpRequestDTO>
    {
        public VerifyOtpValidator()
        {
            RuleFor(x => x.ChallengeToken)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.InvalidChallenge);

            RuleFor(x => x.Otp)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.OtpRequired)
                .Length(6)
                .WithMessage(MessageConstants.FaceAuth.OtpInvalidLength)
                .Matches(@"^\d{6}$")
                .WithMessage(MessageConstants.FaceAuth.PinMustBeDigits);
        }
    }
}
