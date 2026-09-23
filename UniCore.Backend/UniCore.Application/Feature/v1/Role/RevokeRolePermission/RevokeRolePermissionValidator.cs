using FluentValidation;

namespace UniCore.Application.Feature.v1.Role.RevokeRolePermission
{
    public class RevokeRolePermissionValidator : AbstractValidator<RevokeRolePermissionRequestDTO>
    {
        public RevokeRolePermissionValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("Role ID is required.");

            RuleFor(x => x.PermissionIds)
                .NotEmpty()
                .WithMessage("Permission IDs list must not be empty.");
        }
    }
}
