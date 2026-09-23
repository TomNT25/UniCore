using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Permission.GrantUserPermission
{
    public class GrantUserPermissionHandler : IRequestHandler<GrantUserPermissionRequestDTO, GrantUserPermissionResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly ICacheService? _cacheService;
        private readonly IValidator<GrantUserPermissionRequestDTO> _validator;

        public GrantUserPermissionHandler(
            IUserRepository userRepository,
            IPermissionRepository permissionRepository,
            IUserPermissionRepository userPermissionRepository,
            IValidator<GrantUserPermissionRequestDTO> validator,
            ICacheService? cacheService = null)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _validator = validator;
            _cacheService = cacheService;
        }

        public async Task<GrantUserPermissionResponseDTO> HandleAsync(GrantUserPermissionRequestDTO request, CancellationToken cancellationToken)
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

            var existingUserPermissions = await _userPermissionRepository.GetByUserIDAsync(request.UserId, cancellationToken);
            var existingPermissionIds = existingUserPermissions.Select(up => up.PermissionId).ToHashSet();

            var newPermissionIds = request.PermissionIds.Where(id => !existingPermissionIds.Contains(id)).Distinct().ToList();
            if (!newPermissionIds.Any())
            {
                return new GrantUserPermissionResponseDTO { Success = true, GrantedCount = 0 };
            }

            var validPermissions = await _permissionRepository.GetByIdsAsync(newPermissionIds, cancellationToken);

            var userPermissionsToAdd = validPermissions.Select(p => new UserPermission
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                PermissionId = p.Id,
                AssignedAt = DateTime.UtcNow,
                IsActive = true
            }).ToList();

            foreach (var up in userPermissionsToAdd)
            {
                await _userPermissionRepository.AddAsync(up, cancellationToken);
            }

            if (_cacheService != null)
            {
                await _cacheService.RemoveAsync($"user_permissions_{request.UserId}", cancellationToken);
            }

            return new GrantUserPermissionResponseDTO
            {
                Success = true,
                GrantedCount = userPermissionsToAdd.Count
            };
        }
    }
}
