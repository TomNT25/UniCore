using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Application.Contract.Util;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Auth.Register
{
    public class RegisterHandler : IRequestHandler<RegisterRequestDTO, RegisterResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IOtpService _otpService;
        private readonly IMapper _mapper;

        public RegisterHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasherService passwordHasherService,
            IOtpService otpService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
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

            var userId = Guid.NewGuid().ToString();
            var newUser = _mapper.Map<UniCore.Application.Entity.User>(request);
            newUser.Id = userId;
            newUser.PasswordHash = _passwordHasherService.HashPassword(request.Password);
            newUser.Provider = AuthConstants.Providers.System;
            newUser.IsActive = true;
            newUser.IsEmailVerified = false;
            newUser.CreatedAt = DateTime.UtcNow;
            newUser.UpdatedAt = DateTime.UtcNow;

            var userRole = new UserRole
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                RoleId = defaultRole.Id,
                AssignedAt = DateTime.UtcNow,
                IsActive = true,
                Role = defaultRole
            };

            using (var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _userRepository.AddAsync(newUser, cancellationToken);
                    await _userRoleRepository.AddAsync(userRole, cancellationToken);
                    await tx.CommitAsync(cancellationToken);
                }
                catch
                {
                    await tx.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            newUser.UserRoles = new List<UserRole> { userRole };

            var otpCode = _otpService.GenerateOtp($"verify_email_{request.Email}");

            var response = _mapper.Map<RegisterResponseDTO>(newUser);
            response.OtpCode = otpCode;

            return response;
        }
    }
}
