using FluentValidation;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.GetUserProfile
{
    public class GetUserProfileHandler : IRequestHandler<GetUserProfileRequestDTO, GetUserProfileResponseDTO>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetUserProfileRequestDTO> _validator;

        public GetUserProfileHandler(
            IUserProfileRepository userProfileRepository,
            IMapper mapper,
            IValidator<GetUserProfileRequestDTO> validator)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetUserProfileResponseDTO> HandleAsync(GetUserProfileRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            return new GetUserProfileResponseDTO
            {
                Profile = profile != null ? _mapper.Map<UserProfileDTO>(profile) : null
            };
        }
    }
}
