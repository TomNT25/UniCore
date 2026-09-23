using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Permission.GetAllPermission
{
    public class GetAllPermissionHandler : IRequestHandler<GetAllPermissionRequestDTO, GetAllPermissionResponseDTO>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IValidator<GetAllPermissionRequestDTO> _validator;

        public GetAllPermissionHandler(
            IPermissionRepository permissionRepository,
            IValidator<GetAllPermissionRequestDTO> validator)
        {
            _permissionRepository = permissionRepository;
            _validator = validator;
        }

        public async Task<GetAllPermissionResponseDTO> HandleAsync(GetAllPermissionRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            Expression<Func<UniCore.Application.Entity.Permission, bool>>? filter = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? null
                : p => (p.Name != null && p.Name.Contains(request.SearchTerm)) || (p.Code != null && p.Code.Contains(request.SearchTerm));

            var pagedResult = await _permissionRepository.GetPageNumberPaginationAsync<PermissionDTO>(
                request,
                filter,
                cancellationToken);

            return new GetAllPermissionResponseDTO
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
