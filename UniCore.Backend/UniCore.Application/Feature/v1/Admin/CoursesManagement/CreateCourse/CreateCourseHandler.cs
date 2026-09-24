using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.CreateCourse
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseRequestDTO, CreateCourseResponseDTO>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCourseRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public CreateCourseHandler(
            ICourseRepository courseRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IValidator<CreateCourseRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<CreateCourseResponseDTO> HandleAsync(CreateCourseRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var existingCode = await _courseRepository.GetByCodeAsync(request.Code.Trim(), cancellationToken);
                if (existingCode != null)
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

            var courseEntity = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Code = string.IsNullOrWhiteSpace(request.Code) ? null : request.Code.Trim(),
                Type = request.Type.Trim(),
                Name = request.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                DepartmentId = request.DepartmentId,
                IsActive = request.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _courseRepository.AddAsync(courseEntity, cancellationToken);

            var created = await _courseRepository.GetByIdAsync(courseEntity.Id, cancellationToken);

            return new CreateCourseResponseDTO
            {
                Course = _mapper.Map<CreateCourseResultDTO>(created ?? courseEntity)
            };
        }
    }
}
