using FluentValidation;

namespace UniCore.Application.Feature.v1.Permission.GetPermissionById
{
    public class GetPermissionByIdValidator : AbstractValidator<GetPermissionByIdRequestDTO>
    {
        public GetPermissionByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Permission ID is required.");
        }
    }
}
