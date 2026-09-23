using FluentValidation;

namespace UniCore.Application.Feature.v1.Role.DeleteRole
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleRequestDTO>
    {
        public DeleteRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Role ID is required.");
        }
    }
}
