using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;


namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails
{
    public class GetCourseDetailsHandler : IRequestHandler<GetCourseDetailsRequestDTO, GetCourseDetailsResponseDTO>
    {
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IValidator<GetCourseDetailsRequestDTO> _validator;
        private readonly IMapper _mapper;
        public GetCourseDetailsHandler(
            
            IValidator<GetCourseDetailsRequestDTO> validator,
            ICourseStudentRepository courseStudentRepository,
            IMapper mapper
            )
        {
            _validator = validator;
            _courseStudentRepository = courseStudentRepository;
            _mapper = mapper;
        }

        public async Task<GetCourseDetailsResponseDTO> HandleAsync(GetCourseDetailsRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }


            var courseDetails = await _courseStudentRepository.GetDetailedStuIdCourseIdsAsync(request.UserID, request.CourseID, cancellationToken);

            if (courseDetails is null)
            {
                throw new NullReferenceException(nameof(courseDetails));
            }

            var outputs = _mapper.Map<GetCourseDetailsResponseDTO>(courseDetails);

            return outputs;
        }

    }

}
