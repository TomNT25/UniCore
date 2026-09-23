using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourseStatus
{
    public class UpdateCourseStatusHandler : IRequestHandler<UpdateCourseStatusRequestDTO, UpdateCourseStatusResponseDTO>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCourseStatusRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public UpdateCourseStatusHandler(
            ICourseRepository courseRepository,
            IMapper mapper,
            IValidator<UpdateCourseStatusRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<UpdateCourseStatusResponseDTO> HandleAsync(UpdateCourseStatusRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var courseEntity = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (courseEntity == null)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.CourseNotFound);
                throw new ValidationException(new[] { new ValidationFailure("Id", msg) });
            }

            courseEntity.IsActive = request.IsActive;
            courseEntity.UpdatedAt = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(courseEntity, cancellationToken);

            var updated = await _courseRepository.GetByIdAsync(courseEntity.Id, cancellationToken);

            return new UpdateCourseStatusResponseDTO
            {
                Course = _mapper.Map<CourseDTO>(updated ?? courseEntity)
            };
        }
    }
}
