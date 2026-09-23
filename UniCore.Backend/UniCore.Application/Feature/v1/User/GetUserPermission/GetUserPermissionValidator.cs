using FluentValidation;

namespace UniCore.Application.Feature.v1.User.GetUserPermission
{
    public class GetUserPermissionValidator : AbstractValidator<GetUserPermissionRequestDTO>
    {
        public GetUserPermissionValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
        }
    }
}
