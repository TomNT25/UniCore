using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllDepartments
{
    public class GetAllDepartmentsValidator : AbstractValidator<GetAllDepartmentsRequestDTO>
    {
        public GetAllDepartmentsValidator()
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
