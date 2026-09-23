using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;


namespace UniCore.Application.Feature.v1.User.GetUserInfo
{
    public class GetUserInfoHandler : IRequestHandler<GetUserInfoRequestDTO, GetUserInfoResponseDTO?>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IValidator<GetUserInfoRequestDTO> _validator;
        private readonly IMapper _mapper;

        public GetUserInfoHandler(
            IUserProfileRepository profileRepository,
            IMapper mapper,
            IValidator<GetUserInfoRequestDTO> validator
            )
        {
            _profileRepository = profileRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<GetUserInfoResponseDTO?> HandleAsync(GetUserInfoRequestDTO request, CancellationToken ct) 
        {
            ValidationResult results = await _validator.ValidateAsync(request, ct);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var userProfile = await _profileRepository.GetByUserIdAsync(request.UserID, ct);

            if (userProfile == null) 
            {
                return null;
            }

            return _mapper.Map<GetUserInfoResponseDTO>(userProfile);

        }

    }
}
