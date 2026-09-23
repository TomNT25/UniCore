using FluentValidation;
using Microsoft.Extensions.Options;
using UniCore.Helper.Constant;
using UniCore.Helper.Options;

namespace UniCore.Application.Feature.v1.Identity.ScanCccd
{
    public class ScanCccdValidator : AbstractValidator<ScanCccdRequestDTO>
    {
        public ScanCccdValidator(IOptions<AiOcrOptions> options)
        {
            var maxBytes = options.Value.MaxImageBytes > 0
                ? options.Value.MaxImageBytes
                : 10 * 1024 * 1024;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User identity is required.");

            RuleFor(x => x.ImageStream)
                .NotNull()
                .WithMessage("Image file is required.");

            RuleFor(x => x.FileLength)
                .GreaterThan(0)
                .WithMessage("Image file is empty.")
                .LessThanOrEqualTo(maxBytes)
                .WithMessage($"Image file cannot exceed {maxBytes} bytes.");

            RuleFor(x => x.ContentType)
                .Must(ct => !string.IsNullOrWhiteSpace(ct) && AiOcrConstants.AllowedContentTypes.Contains(ct))
                .WithMessage("Image must be image/png, image/jpeg, or image/jpg.");

            RuleFor(x => x.FileName)
                .Must(name =>
                {
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        return false;
                    }

                    var ext = Path.GetExtension(name);
                    return AiOcrConstants.AllowedExtensions.Contains(ext);
                })
                .WithMessage("Image file extension must be .png, .jpg, or .jpeg.");
        }
    }
}
