using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers
{
    public class GetAllUsersValidator : AbstractValidator<GetAllUsersRequestDTO>
    {
        public GetAllUsersValidator()
        {
        }
    }
}
