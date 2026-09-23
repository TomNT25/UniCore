using FluentValidation;

namespace UniCore.Application.Feature.v1.Rbac.GetUserAccessMatrix
{
    public class GetUserAccessMatrixValidator : AbstractValidator<GetUserAccessMatrixRequestDTO>
    {
        public GetUserAccessMatrixValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}
