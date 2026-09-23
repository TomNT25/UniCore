using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequestDTO, UpdateUserResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public UpdateUserHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UpdateUserRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<UpdateUserResponseDTO> HandleAsync(UpdateUserRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (userEntity == null)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.UserNotFound);
                throw new ValidationException(new[] { new ValidationFailure("Id", msg) });
            }

            var existingEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingEmail != null && existingEmail.Id != request.Id)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.EmailAlreadyExists);
                throw new ValidationException(new[] { new ValidationFailure("Email", msg) });
            }

            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (existingUsername != null && existingUsername.Id != request.Id)
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

            userEntity.Username = request.Username;
            userEntity.Email = request.Email;
            userEntity.IsActive = request.IsActive;
            userEntity.IsEmailVerified = request.IsEmailVerified;
            userEntity.UpdatedAt = DateTime.UtcNow;

            var existingUserRoles = await _userRoleRepository.GetByUserIdAsync(userEntity.Id, cancellationToken);
            var rolesToKeepOrUpdate = new List<UserRole>();

            using (var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _userRepository.UpdateAsync(userEntity, cancellationToken);

                    var hasMatchingRole = existingUserRoles.Any(ur => ur.RoleId == request.RoleId);
                    if (!hasMatchingRole)
                    {
                        foreach (var ur in existingUserRoles)
                        {
                            await _userRoleRepository.DeleteAsync(ur, cancellationToken);
                        }

                        var newRole = new UserRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = userEntity.Id,
                            RoleId = request.RoleId,
                            AssignedAt = DateTime.UtcNow,
                            IsActive = true,
                            Role = role
                        };
                        await _userRoleRepository.AddAsync(newRole, cancellationToken);
                        rolesToKeepOrUpdate.Add(newRole);
                    }
                    else
                    {
                        foreach (var ur in existingUserRoles)
                        {
                            if (ur.RoleId == request.RoleId)
                            {
                                ur.Role = role;
                                rolesToKeepOrUpdate.Add(ur);
                            }
                            else
                            {
                                await _userRoleRepository.DeleteAsync(ur, cancellationToken);
                            }
                        }
                    }

                    await tx.CommitAsync(cancellationToken);
                }
                catch
                {
                    await tx.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            userEntity.UserRoles = rolesToKeepOrUpdate;

            return new UpdateUserResponseDTO
            {
                User = _mapper.Map<UserDTO>(userEntity)
            };
        }
    }
}
