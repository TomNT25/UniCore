using FluentValidation;
using FluentValidation.Results;
using Mapster;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Feature.v1.Role.GetAllRole;

namespace UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllStudents
{
    public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsRequestDTO, GetAllStudentsResponseDTO>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IValidator<GetAllStudentsRequestDTO> _validator;

        public GetAllStudentsHandler(
            IUserProfileRepository userProfileRepository,
            IValidator<GetAllStudentsRequestDTO> validator)
        {
            _userProfileRepository = userProfileRepository;
            _validator = validator;
        }

        public async Task<GetAllStudentsResponseDTO> HandleAsync(GetAllStudentsRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            Expression<Func<UniCore.Application.Entity.UserProfile, bool>>? filter = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? null
                : r => (r.FullName != null && r.FullName.Contains(request.SearchTerm)) || (r.Code != null && r.Code.Contains(request.SearchTerm));

            var pagedResult = await _userProfileRepository.GetPageNumberPaginationAsync<GetAllStudentsDTO>(
                request,
                filter,
                cancellationToken);

            return new GetAllStudentsResponseDTO
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
