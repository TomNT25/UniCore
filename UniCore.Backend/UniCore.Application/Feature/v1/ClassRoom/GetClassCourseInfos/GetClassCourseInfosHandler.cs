using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos
{
    public class GetClassCourseInfosHandler : IRequestHandler<GetClassCourseInfosRequestDTO, GetClassCourseInfosResponseDTO>
    {
        private readonly IValidator<GetClassCourseInfosRequestDTO> _validator;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly ICourseRepository _courseRepository;

        public GetClassCourseInfosHandler(
            IValidator<GetClassCourseInfosRequestDTO> validator,
            ICourseStudentRepository courseStudentRepository,
            ICourseRepository courseRepository
            )
        {
            _validator = validator;
            _courseStudentRepository = courseStudentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<GetClassCourseInfosResponseDTO> HandleAsync(GetClassCourseInfosRequestDTO request, CancellationToken ct = default)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }


            //var classInfos = await _schoolClassRepo.GetInfoByIdAsync(request.ClassID, ct);

            var courseIds = await _courseStudentRepository.GetCourseIdsByStuIdAsync(request.UserID, ct);

            if (courseIds is null)
            {
                throw new NullReferenceException(nameof(courseIds));
            }

            var classInfos = await _courseRepository.GetCourseInfosByIdsAsync(courseIds, ct);

            var classInfoList = classInfos.Adapt<IEnumerable<GetClassCourseInfosDTO>>();

            return new GetClassCourseInfosResponseDTO { ClassCourseInfos = classInfoList };
        }

    }
}
