using FluentValidation;

namespace UniCore.Application.Feature.v1.Role.GetRoleById
{
    public class GetRoleByIdValidator : AbstractValidator<GetRoleByIdRequestDTO>
    {
        public GetRoleByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Role ID is required.");
        }
    }
}
