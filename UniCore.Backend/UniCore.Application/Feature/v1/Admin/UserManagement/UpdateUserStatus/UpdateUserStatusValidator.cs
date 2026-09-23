using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus
{
    public class UpdateUserStatusValidator : AbstractValidator<UpdateUserStatusRequestDTO>
    {
        public UpdateUserStatusValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.")
                .MaximumLength(50).WithMessage("User ID cannot exceed 50 characters.");
        }
    }
}
