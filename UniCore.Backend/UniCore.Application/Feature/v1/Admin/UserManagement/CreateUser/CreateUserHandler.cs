using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Application.Contract.Util;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;
using UserEntity = UniCore.Application.Entity.User;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequestDTO, CreateUserResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public CreateUserHandler(
            IUserRepository userRepository, 
            IUserProfileRepository userProfileRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasherService passwordHasherService,
            IMapper mapper,
            IValidator<CreateUserRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _passwordHasherService = passwordHasherService;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<CreateUserResponseDTO> HandleAsync(CreateUserRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var existingEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingEmail != null)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.EmailAlreadyExists);
                throw new ValidationException(new[] { new ValidationFailure("Email", msg) });
            }

            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (existingUsername != null)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.UsernameAlreadyExists);
                throw new ValidationException(new[] { new ValidationFailure("Username", msg) });
            }

            var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
            if (role == null)
            {
                var msg = _localizer.GetString(MessageConstants.Role.NotFound);
                throw new ValidationException(new[] { new ValidationFailure("RoleId", msg) });
            }

            var userId = Guid.NewGuid().ToString();
            var userEntity = new UserEntity
            {
                Id = userId,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _passwordHasherService.HashPassword(request.Password),
                IsActive = request.IsActive,
                IsEmailVerified = true,
                EmailVerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            var fullName = string.Join(" ", new[] { request.FirstName, request.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
            var profileEntity = new UserProfile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FullName = string.IsNullOrWhiteSpace(fullName) ? null : fullName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var userRole = new UserRole
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                RoleId = request.RoleId,
                AssignedAt = DateTime.UtcNow,
                IsActive = true,
                Role = role
            };

            using (var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _userRepository.AddAsync(userEntity, cancellationToken);
                    await _userProfileRepository.AddAsync(profileEntity, cancellationToken);
                    await _userRoleRepository.AddAsync(userRole, cancellationToken);
                    await tx.CommitAsync(cancellationToken);
                }
                catch
                {
                    await tx.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            userEntity.UserProfile = profileEntity;
            userEntity.UserRoles = new List<UserRole> { userRole };

            return new CreateUserResponseDTO
            {
                User = _mapper.Map<UserDTO>(userEntity)
            };
        }
    }
}
