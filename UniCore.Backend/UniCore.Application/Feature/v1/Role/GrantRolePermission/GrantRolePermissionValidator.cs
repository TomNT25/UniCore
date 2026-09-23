using FluentValidation;

namespace UniCore.Application.Feature.v1.Role.GrantRolePermission
{
    public class GrantRolePermissionValidator : AbstractValidator<GrantRolePermissionRequestDTO>
    {
        public GrantRolePermissionValidator()
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
