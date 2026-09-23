using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Auth.Login
{
    public record GetUserByEmailQuery(string Email) : IRequest<UserDTO?>;

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDTO?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByEmailQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO?> HandleAsync(GetUserByEmailQuery request, CancellationToken cancellationToken = default)
        {
            var result = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            return result == null ? null : _mapper.Map<UserDTO>(result);
        }
    }
}