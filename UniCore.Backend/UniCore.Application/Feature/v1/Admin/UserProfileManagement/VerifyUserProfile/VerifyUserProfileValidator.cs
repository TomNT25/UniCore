using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.VerifyUserProfile
{
    public class VerifyUserProfileValidator : AbstractValidator<VerifyUserProfileRequestDTO>
    {
        public VerifyUserProfileValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
