using FluentValidation;

namespace UniCore.Application.Feature.v1.User.GetUserInfo
{
    public class GetUserInfoValidator : AbstractValidator<GetUserInfoRequestDTO>
    {
        public GetUserInfoValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
        }
    }
}
