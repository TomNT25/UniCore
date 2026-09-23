using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsHandler : IRequestHandler<GetAllStudentsRequestDTO, IEnumerable<GetAllStudentsResponseDTO>>
    {
        private readonly IStudentClassRepository _studentClassRepo;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IValidator<GetAllStudentsRequestDTO> _validator;

        public GetClassFriendsHandler(
            IStudentClassRepository studentClassRepo,
            IUserProfileRepository profileRepository,
            IValidator<GetAllStudentsRequestDTO> validator
            )
        {
            _studentClassRepo = studentClassRepo;
            _validator = validator;
            _profileRepository = profileRepository;
        }

        public async Task<IEnumerable<GetAllStudentsResponseDTO>> HandleAsync(GetAllStudentsRequestDTO request, CancellationToken ct)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var classmateId = await _studentClassRepo.GetUserIdsByStudentClassIdAsync(request.ClassID, request.UserID, ct);

            if (classmateId == null)
            {
                throw new NullReferenceException(nameof(classmateId));
            }

            var userInfos = await _profileRepository.GetInfoByIdAsync(classmateId, ct);

            if (userInfos is null)
            {
                throw new NullReferenceException(nameof(userInfos));
            }

            return userInfos.Adapt<IEnumerable<GetAllStudentsResponseDTO>>();

        }

    }
}
