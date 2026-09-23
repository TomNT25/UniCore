using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.Enroll
{
    public class FaceEnrollValidator : AbstractValidator<FaceEnrollRequestDTO>
    {
        public FaceEnrollValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.UserIdRequired);

            RuleFor(x => x.FaceImages)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.ImagesRequired)
                .Must(images => images.Count >= 1 && images.Count <= 5)
                .WithMessage(MessageConstants.FaceAuth.InvalidImageCount);

            RuleForEach(x => x.FaceImages)
                .ChildRules(image =>
                {
                    image.RuleFor(i => i.ContentType)
                        .Must(ct => FaceAuthConstants.AllowedContentTypes.Contains(ct))
                        .WithMessage(MessageConstants.FaceAuth.InvalidImageType);

                    image.RuleFor(i => i.Length)
                        .LessThanOrEqualTo(10 * 1024 * 1024) // 10 MB
                        .WithMessage(MessageConstants.FaceAuth.ImageTooLarge);
                });
        }
    }
}
