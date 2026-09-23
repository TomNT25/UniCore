using FluentValidation;

namespace UniCore.Application.Feature.v1.Permission.RevokeUserPermission
{
    public class RevokeUserPermissionValidator : AbstractValidator<RevokeUserPermissionRequestDTO>
    {
        public RevokeUserPermissionValidator()
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
