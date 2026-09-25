using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.User.PostUserInfo
{
    public class PostUserInfoHandler : IRequestHandler<PostUserInfoRequestDTO, PostUserInfoResponseDTO>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IValidator<PostUserInfoRequestDTO> _validator;
        private readonly IMapper _mapper;

        public PostUserInfoHandler(
            IUserProfileRepository profileRepository,
            IMapper mapper,
            IValidator<PostUserInfoRequestDTO> validator
            )
        {
            _profileRepository = profileRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<PostUserInfoResponseDTO> HandleAsync(PostUserInfoRequestDTO request, CancellationToken cancellationToken) 
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            // var userProfile = await _profileRepository.GetByUserIdAsync(request.UserID, cancellationToken);


            // if (userProfile is null) 
            // {
            //     throw new NullReferenceException();
            // }


            // request.PostUserBio.Adapt(userProfile);

            var userProfile = request.Adapt<UserProfile>();

            await _profileRepository.AddAsync(userProfile, cancellationToken);

            return _mapper.Map<PostUserInfoResponseDTO>(userProfile);

        }

    }
}
