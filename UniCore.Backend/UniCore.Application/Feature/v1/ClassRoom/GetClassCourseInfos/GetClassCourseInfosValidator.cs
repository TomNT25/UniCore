using FluentValidation;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos
{
    public class GetClassCourseInfosValidator : AbstractValidator<GetClassCourseInfosRequestDTO>
    {
        public GetClassCourseInfosValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
            
        }  
    }
}
