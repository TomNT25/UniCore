using FluentValidation;

namespace UniCore.Application.Feature.v1.FaceAuth.ResendOtp
{
    public class ResendOtpValidator : AbstractValidator<ResendOtpRequestDTO>
    {
        public ResendOtpValidator()
        {
            RuleFor(x => x.ChallengeToken)
                .NotEmpty()
                .WithMessage("Challenge token is required");
        }
    }
}
