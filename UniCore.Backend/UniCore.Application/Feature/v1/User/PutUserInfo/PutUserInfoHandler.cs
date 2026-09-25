using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;


namespace UniCore.Application.Feature.v1.User.PutUserInfo
{
    public class PutUserInfoHandler : IRequestHandler<PutUserInfoRequestDTO, PutUserInfoResponseDTO>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IValidator<PutUserInfoRequestDTO> _validator;
        private readonly IMapper _mapper;

        public PutUserInfoHandler(
            IUserProfileRepository profileRepository,
            IMapper mapper,
            IValidator<PutUserInfoRequestDTO> validator
            )
        {
            _profileRepository = profileRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<PutUserInfoResponseDTO> HandleAsync(PutUserInfoRequestDTO request, CancellationToken cancellationToken) 
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var userProfile = await _profileRepository.GetByUserIdAsync(request.UserID, cancellationToken);


            if (userProfile is null) 
            {
                throw new NullReferenceException();
            }

            request.PutUserBio.Adapt(userProfile);

            return userProfile.Adapt<PutUserInfoResponseDTO>();

        }

    }
}
