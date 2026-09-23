using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.DeleteCourse
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseRequestDTO, DeleteCourseResponseDTO>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IValidator<DeleteCourseRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public DeleteCourseHandler(
            ICourseRepository courseRepository,
            IValidator<DeleteCourseRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _courseRepository = courseRepository;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<DeleteCourseResponseDTO> HandleAsync(DeleteCourseRequestDTO request, CancellationToken cancellationToken)
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

            courseEntity.IsDeleted = true;
            courseEntity.IsActive = false;
            courseEntity.UpdatedAt = DateTime.UtcNow;

            var success = await _courseRepository.UpdateAsync(courseEntity, cancellationToken);

            return new DeleteCourseResponseDTO
            {
                Success = success
            };
        }
    }
}
