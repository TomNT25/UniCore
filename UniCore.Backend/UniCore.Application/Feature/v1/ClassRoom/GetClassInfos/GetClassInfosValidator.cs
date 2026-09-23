using FluentValidation;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class GetClassInfoValidator : AbstractValidator<GetClassInfoRequestDTO>
    {
        public GetClassInfoValidator()
        {
            RuleFor(x => x.ClassID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
            
        }  
    }
}
