using FluentValidation;

namespace UniCore.Application.Feature.v1.Role.UpdateRole
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleRequestDTO>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Role ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Role Name is required.");
        }
    }
}
