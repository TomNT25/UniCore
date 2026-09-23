using FluentValidation;

namespace UniCore.Application.Feature.v1.Role.RevokeUserRole
{
    public class RevokeUserRoleValidator : AbstractValidator<RevokeUserRoleRequestDTO>
    {
        public RevokeUserRoleValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.RoleIds)
                .NotEmpty()
                .WithMessage("Role IDs list must not be empty.");
        }
    }
}
