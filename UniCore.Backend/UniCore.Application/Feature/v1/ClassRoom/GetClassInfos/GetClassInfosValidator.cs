using FluentValidation;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class GetClassInfoValidator : AbstractValidator<GetClassInfoRequestDTO>
    {
        public GetClassInfoValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
            
        }  
    }
}
