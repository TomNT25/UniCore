using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.DeleteUser
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserRequestDTO>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
