using FluentValidation;
using UniCore.Application.Feature.v1.Identity.GetMyPersonId;

namespace UniCore.Application.Feature.v1.Identity.GetMyPersonId
{
    public class GetMyPersonIdValidator : AbstractValidator<GetMyPersonIdRequestDTO>
    {
        public GetMyPersonIdValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User identity is required.");
        }
    }
}
