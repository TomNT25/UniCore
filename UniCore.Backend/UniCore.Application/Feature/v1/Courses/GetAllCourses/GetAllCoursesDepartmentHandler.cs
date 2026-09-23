using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;


namespace UniCore.Application.Feature.v1.Courses.GetAllCourses
{
    public class GetAllCoursesDepartmentHandler : IRequestHandler<GetAllCoursesRequestDTO, GetAllCoursesResponseDTO>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IValidator<GetAllCoursesRequestDTO> _validator;

        public GetAllCoursesDepartmentHandler(
            ICourseRepository courseRepository,
            IValidator<GetAllCoursesRequestDTO> validator)
        {
            _courseRepository = courseRepository;
            _validator = validator;
        }

        public async Task<GetAllCoursesResponseDTO> HandleAsync(GetAllCoursesRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            Expression<Func<UniCore.Application.Entity.Course, bool>>? filter = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? null
                : r => (r.Name != null && r.Name.Contains(request.SearchTerm)) || 
                       (r.Code != null && r.Code.Contains(request.SearchTerm)) ||
                       (r.Department.Code != null && r.Department.Code.Contains(request.SearchTerm));

            var pagedResult = await _courseRepository.GetPageNumberPaginationAsync<GetAllCoursesDTO>(
                request,
                filter,
                cancellationToken);

            return new GetAllCoursesResponseDTO
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
