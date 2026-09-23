using FluentValidation;

namespace UniCore.Application.Feature.v1.Permission.UpdatePermission
{
    public class UpdatePermissionValidator : AbstractValidator<UpdatePermissionRequestDTO>
    {
        public UpdatePermissionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Permission ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Permission Name is required.");

            RuleFor(x => x.Resource)
                .NotEmpty()
                .WithMessage("Resource is required.");

            RuleFor(x => x.Action)
                .NotEmpty()
                .WithMessage("Action is required.");
        }
    }
}
