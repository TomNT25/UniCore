using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Role.GrantUserRole
{
    public class GrantUserRoleHandler : IRequestHandler<GrantUserRoleRequestDTO, GrantUserRoleResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ICacheService? _cacheService;
        private readonly IValidator<GrantUserRoleRequestDTO> _validator;

        public GrantUserRoleHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IValidator<GrantUserRoleRequestDTO> validator,
            ICacheService? cacheService = null)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _validator = validator;
            _cacheService = cacheService;
        }

        public async Task<GrantUserRoleResponseDTO> HandleAsync(GrantUserRoleRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
            }

            var existingUserRoles = await _userRoleRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            var existingRoleIds = existingUserRoles.Select(ur => ur.RoleId).ToHashSet();

            var newRoleIds = request.RoleIds.Where(id => !existingRoleIds.Contains(id)).Distinct().ToList();
            if (!newRoleIds.Any())
            {
                return new GrantUserRoleResponseDTO { Success = true, GrantedCount = 0 };
            }

            var userRolesToAdd = newRoleIds.Select(roleId => new UserRole
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow,
                IsActive = true
            }).ToList();

            foreach (var ur in userRolesToAdd)
            {
                await _userRoleRepository.AddAsync(ur, cancellationToken);
            }

            if (_cacheService != null)
            {
                await _cacheService.RemoveAsync($"user_permissions_{request.UserId}", cancellationToken);
            }

            return new GrantUserRoleResponseDTO
            {
                Success = true,
                GrantedCount = userRolesToAdd.Count
            };
        }
    }
}
