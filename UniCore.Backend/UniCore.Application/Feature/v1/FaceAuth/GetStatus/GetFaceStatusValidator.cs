using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.GetStatus
{
    public class GetFaceStatusValidator : AbstractValidator<GetFaceStatusRequestDTO>
    {
        public GetFaceStatusValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(MessageConstants.FaceAuth.UserIdRequired);
        }
    }
}
