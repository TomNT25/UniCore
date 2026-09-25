using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsHandler : IRequestHandler<GetClassFriendsRequestDTO, GetClassFriendsResponseDTO>
    {
        private readonly IValidator<GetClassFriendsRequestDTO> _validator;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public GetClassFriendsHandler(
            ISchoolClassRepository schoolClassRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<GetClassFriendsRequestDTO> validator
            )
        {
            _schoolClassRepository = schoolClassRepository;
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<GetClassFriendsResponseDTO> HandleAsync(GetClassFriendsRequestDTO request, CancellationToken ct)
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var user = await _userRepository.GetByIdAsync(request.UserID, ct);

            if (user?.ClassId is null)
            {
                throw new NullReferenceException(nameof(user));
            }

            var classID = user.ClassId;

            var classInfo = await _schoolClassRepository.GetClassmatesByClassIdAsync(classID, request.UserID, ct);

            if (classInfo is null)
            {
                throw new NullReferenceException(nameof(classInfo));
            }

            var response = new GetClassFriendsResponseDTO
            {
                ClassmatesList = _mapper.Map<IEnumerable<GetClassFriendsDTO>>(classInfo.Students)
            };

            return response;
        }

    }
}
