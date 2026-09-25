using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class GetClassInfoHandler : IRequestHandler<GetClassInfoRequestDTO, GetClassInfoResponseDTO>
    {
        private readonly ISchoolClassRepository _schoolClassRepo;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<GetClassInfoRequestDTO> _validator;

        public GetClassInfoHandler(
            ISchoolClassRepository schoolClassRepo,
            IUserRepository userRepository,
            ICourseStudentRepository courseStudentRepository,
            IValidator<GetClassInfoRequestDTO> validator
            )
        {
            _validator = validator;
            _userRepository = userRepository;
            _schoolClassRepo = schoolClassRepo;
            _courseStudentRepository = courseStudentRepository;
        }

        public async Task<GetClassInfoResponseDTO> HandleAsync(GetClassInfoRequestDTO request, CancellationToken ct)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var user = await _userRepository.GetByIdAsync(request.UserID, ct);

            if(user?.ClassId is null)
            {
                throw new NullReferenceException(nameof(user));
            }

            Expression<Func<Entity.CourseStudent, bool>>? filter = null;

            var rq = new UniCore.Application.DTO.PageNumberPaginationRequest
            {
                PageNumber = 1,
                PageSize = 100000
            };

            var pagedResult = await _courseStudentRepository.GetPaginatedByStuIdCourseIdsAsync(request.UserID,
            rq, ct, filter);

            var score = pagedResult.Items.Sum(x => x.Course.CourseStudents.Select(cs => cs.FinalScore).FirstOrDefault() * 
                                        x.Course.CourseStudents.Select(cs => cs.Weight).FirstOrDefault()) / 
                                        pagedResult.Items.Sum(x => x.Course.CourseStudents.Select(cs => cs.Weight).FirstOrDefault());

            var classID = user.ClassId;

            var classInfos = await _schoolClassRepo.GetInfoByIdAsync(classID, ct);

            if (classInfos is null)
            {
                throw new NullReferenceException(nameof(classInfos));
            }

            var response = classInfos.Adapt<GetClassInfoResponseDTO>();
            response.Score = score ?? 0m;

            return response;
        }

    }
}
