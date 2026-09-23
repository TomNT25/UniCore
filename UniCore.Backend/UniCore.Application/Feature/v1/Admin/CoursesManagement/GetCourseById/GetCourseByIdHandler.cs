using FluentValidation;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.CoursesManagement.GetCourseById
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdRequestDTO, GetCourseByIdResponseDTO>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCourseByIdRequestDTO> _validator;

        public GetCourseByIdHandler(
            ICourseRepository courseRepository,
            IMapper mapper,
            IValidator<GetCourseByIdRequestDTO> validator)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetCourseByIdResponseDTO> HandleAsync(GetCourseByIdRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var courseEntity = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
            return new GetCourseByIdResponseDTO
            {
                Course = courseEntity != null ? _mapper.Map<CourseDTO>(courseEntity) : null
            };
        }
    }
}
