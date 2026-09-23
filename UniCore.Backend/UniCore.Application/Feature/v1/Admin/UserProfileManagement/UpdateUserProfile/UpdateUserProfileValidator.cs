using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileRequestDTO>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
