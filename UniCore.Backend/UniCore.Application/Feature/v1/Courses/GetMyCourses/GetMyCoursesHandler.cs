using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;



namespace UniCore.Application.Feature.v1.Courses.GetMyCourses
{
    public class GetMyCoursesHandler : IRequestHandler<GetMyCoursesRequestDTO, GetMyCoursesResponseDTO>
    {
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IValidator<GetMyCoursesRequestDTO> _validator;

        public GetMyCoursesHandler(
            ICourseStudentRepository courseStudentRepository,
            IValidator<GetMyCoursesRequestDTO> validator
            )
        {
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
                Items = pagedResult.Items.Adapt<List<GetMyCoursesDTO>>(),
                Metadata = pagedResult.Metadata
            };
        }
    }
}


