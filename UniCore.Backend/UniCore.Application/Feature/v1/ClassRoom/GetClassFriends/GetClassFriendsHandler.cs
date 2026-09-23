using FluentValidation;
using FluentValidation.Results;
using Mapster;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsHandler : IRequestHandler<GetClassFriendsRequestDTO, GetClassFriendsResponseDTO>
    {
        private readonly IStudentClassRepository _studentClassRepo;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IValidator<GetClassFriendsRequestDTO> _validator;

        public GetClassFriendsHandler(
            IStudentClassRepository studentClassRepo,
            IUserProfileRepository profileRepository,
            IValidator<GetClassFriendsRequestDTO> validator
            )
        {
            _studentClassRepo = studentClassRepo;
            _validator = validator;
            _profileRepository = profileRepository;
        }

        public async Task<GetClassFriendsResponseDTO> HandleAsync(GetClassFriendsRequestDTO request, CancellationToken ct)
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

            var classmateLists = userInfos.Adapt<IEnumerable<GetClassFriendsDTO>>();
            return new GetClassFriendsResponseDTO() { ClassmatesList = classmateLists };
        }

    }
}
