using FluentValidation;

namespace UniCore.Application.Feature.v1.User.PutUserInfo
{
    public class PutUserInfoValidator : AbstractValidator<PutUserInfoRequestDTO>
    {
        public PutUserInfoValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
            RuleFor(x => x.PutUserBio.Bio)
                .NotEmpty()
                .WithMessage("Bio cannot be empty");
        }
    }
}
