using FluentValidation;
using FluentValidation.Results;
using Mapster;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;



namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
        public class GetMyCoursesHandler : IRequestHandler<GetMyCoursesRequestDTO, GetMyCoursesResponseDTO>
        {
            private readonly ICourseRepository _courseRepository;

            private readonly ICourseStudentRepository _courseStudentRepository;
            private readonly IValidator<GetMyCoursesRequestDTO> _validator;

            public GetMyCoursesHandler(
                ICourseRepository courseRepository,
                ICourseStudentRepository courseStudentRepository,
                IValidator<GetMyCoursesRequestDTO> validator
                
                )
            {
                _courseRepository = courseRepository;
                _validator = validator;
                _courseStudentRepository = courseStudentRepository;
            }

            public async Task<GetMyCoursesResponseDTO> HandleAsync(GetMyCoursesRequestDTO request, CancellationToken cancellationToken)
            {
                ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

                if (!results.IsValid)
                {
                    throw new ValidationException(results.Errors);
                }


            Expression<Func<UniCore.Application.Entity.CourseStudent, bool>>? filter = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? null
                : r => (r.Code != null && r.Code.Contains(request.SearchTerm));

                var pagedResult = await _courseStudentRepository.GetPaginatedByStuIdCourseIdsAsync(request.UserID,
                request, cancellationToken);

            return new GetMyCoursesResponseDTO
                {
                    Items = pagedResult.Adapt<IEnumerable<GetMyCoursesDTO>>(),
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


