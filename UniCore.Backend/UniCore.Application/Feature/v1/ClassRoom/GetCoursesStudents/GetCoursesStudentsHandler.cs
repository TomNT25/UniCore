using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents
{
    public class GetCoursesStudentsHandler : IRequestHandler<GetCoursesStudentsRequestDTO, IEnumerable<GetCoursesStudentsResponseDTO>>
    {
        private readonly ICourseStudentRepository _courseStudent;
        private readonly IValidator<GetCoursesStudentsRequestDTO> _validator;

        public GetCoursesStudentsHandler(

            ICourseStudentRepository courseStudent,
            IValidator<GetCoursesStudentsRequestDTO> validator
            )
        {
            _validator = validator;
            _courseStudent = courseStudent;
        }

        public async Task<IEnumerable<GetCoursesStudentsResponseDTO>> HandleAsync(GetCoursesStudentsRequestDTO request, CancellationToken ct = default)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var courseStudentInfo = await _courseStudent.GetByStuIdCourseIdsAsync(request.UserID, request.CourseIDs, ct);

            if (courseStudentInfo is null)
            {
                throw new NullReferenceException(nameof(courseStudentInfo));
            }

            return courseStudentInfo.Adapt<IEnumerable<GetCoursesStudentsResponseDTO>>();

        }

    }
}
