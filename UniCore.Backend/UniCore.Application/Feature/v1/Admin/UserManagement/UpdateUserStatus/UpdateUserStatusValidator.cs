using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus
{
    public class UpdateUserStatusValidator : AbstractValidator<UpdateUserStatusRequestDTO>
    {
        public UpdateUserStatusValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
