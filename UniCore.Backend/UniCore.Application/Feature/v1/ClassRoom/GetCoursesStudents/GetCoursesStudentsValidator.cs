using FluentValidation;

namespace UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents
{
    public class GetCoursesStudentsValidator : AbstractValidator<GetCoursesStudentsRequestDTO>
    {
        public GetCoursesStudentsValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty()
                .WithMessage("Please specify a User ID");
            
        }  
    }
}
