using FluentValidation;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetAllStudentsValidator : AbstractValidator<GetAllStudentsRequestDTO>
    {
        public GetAllStudentsValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
            RuleFor(x => x.ClassID)
                .NotEmpty()
                .WithMessage("Please specify a Class ID");
            
        }  
    }
}
