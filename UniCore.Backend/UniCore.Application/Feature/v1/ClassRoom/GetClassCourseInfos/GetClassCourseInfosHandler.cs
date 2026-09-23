using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos
{
    public class GetClassCourseInfosHandler : IRequestHandler<GetClassCourseInfosRequestDTO, IEnumerable<GetClassCourseInfosResponseDTO>>
    {
        private readonly ISchoolClassRepository _schoolClassRepo;
        private readonly ICourseRepository _courseRepository;
        private readonly IValidator<GetClassCourseInfosRequestDTO> _validator;

        public GetClassCourseInfosHandler(
            ISchoolClassRepository schoolClassRepo,
            ICourseRepository courseRepository,
            IValidator<GetClassCourseInfosRequestDTO> validator
            )
        {
            _validator = validator;
            _courseRepository = courseRepository;
            _schoolClassRepo = schoolClassRepo;
        }

        public async Task<IEnumerable<GetClassCourseInfosResponseDTO>> HandleAsync(GetClassCourseInfosRequestDTO request, CancellationToken ct = default)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }


            //var classInfos = await _schoolClassRepo.GetInfoByIdAsync(request.ClassID, ct);

            var classInfos = await _courseRepository.GetCourseInfosByIds(request.CourseIds, ct);

            if (classInfos is null)
            {
                throw new NullReferenceException(nameof(classInfos));
            }

            return classInfos.Adapt<IEnumerable<GetClassCourseInfosResponseDTO>>();

        }

    }
}
