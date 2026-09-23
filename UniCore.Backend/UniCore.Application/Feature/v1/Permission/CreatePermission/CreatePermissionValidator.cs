using FluentValidation;

namespace UniCore.Application.Feature.v1.Permission.CreatePermission
{
    public class CreatePermissionValidator : AbstractValidator<CreatePermissionRequestDTO>
    {
        public CreatePermissionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Permission Name is required.");

            RuleFor(x => x.Resource)
                .NotEmpty()
                .WithMessage("Resource name is required.");

            RuleFor(x => x.Action)
                .NotEmpty()
                .WithMessage("Action name is required.");
        }
    }
}
