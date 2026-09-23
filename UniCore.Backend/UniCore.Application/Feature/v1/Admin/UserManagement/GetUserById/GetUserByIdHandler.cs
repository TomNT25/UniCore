using FluentValidation;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequestDTO, GetUserByIdResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetUserByIdRequestDTO> _validator;

        public GetUserByIdHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<GetUserByIdRequestDTO> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetUserByIdResponseDTO> HandleAsync(GetUserByIdRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            return new GetUserByIdResponseDTO
            {
                User = userEntity != null ? _mapper.Map<UserDTO>(userEntity) : null
            };
        }
    }
}
