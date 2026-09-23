using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;


namespace UniCore.Application.Feature.v1.Classes.GetClasses
{
    public class GetClassesByNamesHandler : IRequestHandler<GetClassesRequestDTO, GetClassesResponseDTO>
    {
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IValidator<GetClassesRequestDTO> _validator;

        public GetClassesByNamesHandler(
            ISchoolClassRepository schoolClassRepository,
            IValidator<GetClassesRequestDTO> validator)
        {
            _schoolClassRepository = schoolClassRepository;
            _validator = validator;
        }

        public async Task<GetClassesResponseDTO> HandleAsync(GetClassesRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            Expression<Func<UniCore.Application.Entity.SchoolClass, bool>>? filter = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? null
                : r => (r.Name != null && r.Name.Contains(request.SearchTerm)) ||
                       (r.Code != null && r.Code.Contains(request.SearchTerm));

            var pagedResult = await _schoolClassRepository.GetPageNumberPaginationAsync<GetClassesDTO>(
                request,
                filter,
                cancellationToken);

            return new GetClassesResponseDTO
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
