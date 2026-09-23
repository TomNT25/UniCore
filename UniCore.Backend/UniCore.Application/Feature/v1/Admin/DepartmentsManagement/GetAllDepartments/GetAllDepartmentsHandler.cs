using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;


namespace UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllDepartments
{
    public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsRequestDTO, GetAllDepartmentsResponseDTO>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IValidator<GetAllDepartmentsRequestDTO> _validator;

        public GetAllDepartmentsHandler(
            IDepartmentRepository departmentRepository,
            IValidator<GetAllDepartmentsRequestDTO> validator)
        {
            _departmentRepository = departmentRepository;
            _validator = validator;
        }

        public async Task<GetAllDepartmentsResponseDTO> HandleAsync(GetAllDepartmentsRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            Expression<Func<UniCore.Application.Entity.Department, bool>>? filter = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? null
                : r => (r.Name != null && r.Name.Contains(request.SearchTerm)) || (r.Code != null && r.Code.Contains(request.SearchTerm));

            var pagedResult = await _departmentRepository.GetPageNumberPaginationAsync<GetAllDepartmentsDTO>(
                request,
                filter,
                cancellationToken);

            return new GetAllDepartmentsResponseDTO
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
