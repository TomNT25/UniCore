using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequestDTO, UpdateUserResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public UpdateUserHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            IValidator<UpdateUserRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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

            await _userRepository.UpdateAsync(userEntity, cancellationToken);
            // userEntity.Role = role;

            return new UpdateUserResponseDTO
            {
                User = _mapper.Map<UserDTO>(userEntity)
            };
        }
    }
}
