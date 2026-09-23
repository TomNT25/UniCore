using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.GetUserProfile
{
    public class GetUserProfileValidator : AbstractValidator<GetUserProfileRequestDTO>
    {
        public GetUserProfileValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
