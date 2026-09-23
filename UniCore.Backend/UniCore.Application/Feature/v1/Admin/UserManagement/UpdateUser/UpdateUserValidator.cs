using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDTO>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role ID is required.");
        }
    }
}
