using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers
{
    public class GetAllUsersValidator : AbstractValidator<GetAllUsersRequestDTO>
    {
        public GetAllUsersValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            When(x => !string.IsNullOrWhiteSpace(x.SearchTerm), () =>
            {
                RuleFor(x => x.SearchTerm)
                    .MaximumLength(100).WithMessage("SearchTerm cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.RoleId), () =>
            {
                RuleFor(x => x.RoleId)
                    .MaximumLength(50).WithMessage("RoleId cannot exceed 50 characters.");
            });
        }
    }
}
