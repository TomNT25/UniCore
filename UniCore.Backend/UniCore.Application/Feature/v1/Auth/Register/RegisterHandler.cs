using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Auth.Register
{
    public class RegisterHandler : IRequestHandler<RegisterRequestDTO, RegisterResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IOtpService _otpService;
        private readonly IMapper _mapper;

        public RegisterHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasherService passwordHasherService,
            IOtpService otpService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasherService = passwordHasherService;
            _otpService = otpService;
            _mapper = mapper;
        }

        public async Task<RegisterResponseDTO> HandleAsync(RegisterRequestDTO request, CancellationToken cancellationToken)
        {
            var existingByEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingByEmail != null)
            {
                throw new InvalidOperationException(MessageConstants.Auth.EmailRegistered);
            }

            var existingByUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (existingByUsername != null)
            {
                throw new InvalidOperationException(MessageConstants.Auth.UsernameTaken);
            }

            var defaultRole = await _roleRepository.GetDefaultRoleAsync(cancellationToken);
            if (defaultRole == null)
            {
                throw new InvalidOperationException(MessageConstants.Auth.DefaultRoleNotFound);
            }

            var newUser = _mapper.Map<UniCore.Application.Entity.User>(request);
            newUser.Id = Guid.NewGuid().ToString();
            newUser.PasswordHash = _passwordHasherService.HashPassword(request.Password);
            newUser.Provider = AuthConstants.Providers.System;
            newUser.IsActive = true;
            newUser.IsEmailVerified = false;
            newUser.CreatedAt = DateTime.UtcNow;
            newUser.UpdatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(newUser, cancellationToken);

            var otpCode = _otpService.GenerateOtp($"verify_email_{request.Email}");

            var response = _mapper.Map<RegisterResponseDTO>(newUser);
            response.OtpCode = otpCode;

            return response;
        }
    }
}
