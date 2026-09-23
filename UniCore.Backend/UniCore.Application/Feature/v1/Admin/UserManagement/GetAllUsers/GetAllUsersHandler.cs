using FluentValidation;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UserEntity = UniCore.Application.Entity.User;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequestDTO, GetAllUsersResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<GetAllUsersRequestDTO> _validator;

        public GetAllUsersHandler(
            IUserRepository userRepository,
            IValidator<GetAllUsersRequestDTO> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<GetAllUsersResponseDTO> HandleAsync(GetAllUsersRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            Expression<Func<UserEntity, bool>>? filter = u =>
                (string.IsNullOrWhiteSpace(request.SearchTerm) ||
                    u.Username.Contains(request.SearchTerm) ||
                    u.Email.Contains(request.SearchTerm) ||
                    (u.Code != null && u.Code.Contains(request.SearchTerm))) &&
                (string.IsNullOrWhiteSpace(request.RoleId)) &&
                (!request.IsActive.HasValue || u.IsActive == request.IsActive.Value) &&
                (!request.IsEmailVerified.HasValue || u.IsEmailVerified == request.IsEmailVerified.Value);

            var pagedResult = await _userRepository.GetPageNumberPaginationAsync<UserDTO>(
                request,
                filter,
                cancellationToken);

            return new GetAllUsersResponseDTO
            {
                Items = pagedResult.Items,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages,
                HasNextPage = pagedResult.HasNextPage,
                HasPreviousPage = pagedResult.HasPreviousPage
            };
        }
    }
}
