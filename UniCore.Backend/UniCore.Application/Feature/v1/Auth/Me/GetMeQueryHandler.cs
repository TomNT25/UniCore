using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Me
{
    public class GetMeQueryHandler : IRequestHandler<GetMeQuery, GetMeResponseDTO?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetMeQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetMeResponseDTO?> HandleAsync(GetMeQuery request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                user = await _userRepository.GetByEmailAsync(request.UserId, cancellationToken);
            }

            return user == null ? null : _mapper.Map<GetMeResponseDTO>(user);
        }
    }
}
