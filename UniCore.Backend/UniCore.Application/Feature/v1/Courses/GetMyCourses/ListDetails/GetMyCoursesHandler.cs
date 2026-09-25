using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Courses.GetMyCourses.InternalDTOs;



namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.ListDetails
{
        public class GetMyCoursesHandler : IRequestHandler<GetMyCoursesRequestDTO, GetMyCoursesResponseDTO>
        {

            private readonly ICourseStudentRepository _courseStudentRepository;
            private readonly IValidator<GetMyCoursesRequestDTO> _validator;
            private readonly IMapper _mapper;

            public GetMyCoursesHandler(
                ICourseRepository courseRepository,
                ICourseStudentRepository courseStudentRepository,
                IValidator<GetMyCoursesRequestDTO> validator,
                IMapper mapper
                
                )
            {

                _validator = validator;
                _courseStudentRepository = courseStudentRepository;
                _mapper = mapper;
            }

            public async Task<GetMyCoursesResponseDTO> HandleAsync(GetMyCoursesRequestDTO request, CancellationToken cancellationToken)
            {
                ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

                if (!results.IsValid)
                {
                    throw new ValidationException(results.Errors);
                }


                Expression<Func<Entity.CourseStudent, bool>>? filter = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? null
                : r => (r.Code != null && r.Code.Contains(request.SearchTerm));

                var pagedResult = await _courseStudentRepository.GetPaginatedByStuIdCourseIdsAsync(request.UserID,
                request,cancellationToken, filter);

            if(pagedResult.Items is null)
            {
                throw new NullReferenceException("There's no Courses for this Student");
            }

            return new GetMyCoursesResponseDTO
                {
                    Items = _mapper.Map<IEnumerable<GetMyCoursesDTO>>(pagedResult.Items),
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


