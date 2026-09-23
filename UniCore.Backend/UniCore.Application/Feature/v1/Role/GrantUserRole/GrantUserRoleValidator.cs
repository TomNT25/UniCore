using FluentValidation;

namespace UniCore.Application.Feature.v1.Role.GrantUserRole
{
    public class GrantUserRoleValidator : AbstractValidator<GrantUserRoleRequestDTO>
    {
        public GrantUserRoleValidator()
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
