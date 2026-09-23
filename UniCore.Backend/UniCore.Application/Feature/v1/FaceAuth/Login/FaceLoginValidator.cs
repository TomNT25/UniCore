using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.Login
{
    public class FaceLoginValidator : AbstractValidator<FaceLoginRequestDTO>
    {
        public FaceLoginValidator()
        {
            RuleFor(x => x.FaceStream)
                .NotNull()
                .WithMessage(MessageConstants.FaceAuth.ImagesRequired);

            RuleFor(x => x.ContentType)
                .Must(ct => FaceAuthConstants.AllowedContentTypes.Contains(ct))
                .WithMessage(MessageConstants.FaceAuth.InvalidImageType);

            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage(MessageConstants.FaceAuth.ImagesRequired)
                .LessThanOrEqualTo(10 * 1024 * 1024)
                .WithMessage(MessageConstants.FaceAuth.ImageTooLarge);
        }
    }
}
