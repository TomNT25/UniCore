using FluentValidation;

namespace UniCore.Application.Feature.v1.Permission.GrantUserPermission
{
    public class GrantUserPermissionValidator : AbstractValidator<GrantUserPermissionRequestDTO>
    {
        public GrantUserPermissionValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.PermissionIds)
                .NotEmpty()
                .WithMessage("Permission IDs list must not be empty.");
        }
    }
}
