using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById
{
    public class GetUserByIdValidator : AbstractValidator<GetUserByIdRequestDTO>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
