using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;


namespace UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails
{
    public class GetCourseDetailsHandler : IRequestHandler<GetCourseDetailsRequestDTO, GetCourseDetailsResponseDTO>
    {
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IValidator<GetCourseDetailsRequestDTO> _validator;

        public GetCourseDetailsHandler(

            IValidator<GetCourseDetailsRequestDTO> validator,
            ICourseStudentRepository courseStudentRepository
            )
        {
            _validator = validator;
            _courseStudentRepository = courseStudentRepository;
        }

        public async Task<GetCourseDetailsResponseDTO> HandleAsync(GetCourseDetailsRequestDTO request, CancellationToken ct)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }


            var courseDetails = await _courseStudentRepository.GetDetailedStuIdCourseIdsAsync(request.UserID, request.CourseID, ct);

            if (courseDetails is null)
            {
                throw new NullReferenceException(nameof(courseDetails));
            }

            return courseDetails.Adapt<GetCourseDetailsResponseDTO>();
        }

    }

}
