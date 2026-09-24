using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourse
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourseRequestDTO, UpdateCourseResponseDTO>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCourseRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public UpdateCourseHandler(
            ICourseRepository courseRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IValidator<UpdateCourseRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<UpdateCourseResponseDTO> HandleAsync(UpdateCourseRequestDTO request, CancellationToken cancellationToken)
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

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var existingCode = await _courseRepository.GetByCodeAsync(request.Code.Trim(), cancellationToken);
                if (existingCode != null && existingCode.Id != request.Id)
                {
                    var msg = _localizer.GetString(MessageConstants.Admin.CourseCodeAlreadyExists);
                    throw new ValidationException(new[] { new ValidationFailure("Code", msg) });
                }
            }

            var departmentExists = await _departmentRepository.ExistsAsync(d => d.Id == request.DepartmentId && !d.IsDeleted, cancellationToken);
            if (!departmentExists)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.DepartmentNotFound);
                throw new ValidationException(new[] { new ValidationFailure("DepartmentId", msg) });
            }

            courseEntity.Code = string.IsNullOrWhiteSpace(request.Code) ? null : request.Code.Trim();
            courseEntity.Type = request.Type.Trim();
            courseEntity.Name = request.Name.Trim();
            courseEntity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            courseEntity.DepartmentId = request.DepartmentId;
            courseEntity.IsActive = request.IsActive;
            courseEntity.UpdatedAt = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(courseEntity, cancellationToken);

            var updated = await _courseRepository.GetByIdAsync(courseEntity.Id, cancellationToken);

            return new UpdateCourseResponseDTO
            {
                Course = _mapper.Map<UpdateCourseResultDTO>(updated ?? courseEntity)
            };
        }
    }
}
