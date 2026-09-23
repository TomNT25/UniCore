using FluentValidation;

namespace UniCore.Application.Feature.v1.Permission.DeletePermission
{
    public class DeletePermissionValidator : AbstractValidator<DeletePermissionRequestDTO>
    {
        public DeletePermissionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Permission ID is required.");
        }
    }
}
