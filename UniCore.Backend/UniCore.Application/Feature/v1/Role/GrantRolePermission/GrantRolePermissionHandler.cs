using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Role.GrantRolePermission
{
    public class GrantRolePermissionHandler : IRequestHandler<GrantRolePermissionRequestDTO, GrantRolePermissionResponseDTO>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ICacheService? _cacheService;
        private readonly IValidator<GrantRolePermissionRequestDTO> _validator;

        public GrantRolePermissionHandler(
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository,
            IUserRoleRepository userRoleRepository,
            IValidator<GrantRolePermissionRequestDTO> validator,
            ICacheService? cacheService = null)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userRoleRepository = userRoleRepository;
            _validator = validator;
            _cacheService = cacheService;
        }

        public async Task<GrantRolePermissionResponseDTO> HandleAsync(GrantRolePermissionRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.RoleId} not found.");
            }

            var existingRolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(request.RoleId, cancellationToken);
            var existingPermissionIds = existingRolePermissions.Select(rp => rp.PermissionId).ToHashSet();

            var newPermissionIds = request.PermissionIds.Where(id => !existingPermissionIds.Contains(id)).Distinct().ToList();
            if (!newPermissionIds.Any())
            {
                return new GrantRolePermissionResponseDTO { Success = true, GrantedCount = 0 };
            }

            var validPermissions = await _permissionRepository.GetByIdsAsync(newPermissionIds, cancellationToken);

            var rolePermissionsToAdd = validPermissions.Select(p => new RolePermission
            {
                Id = Guid.NewGuid().ToString(),
                RoleId = request.RoleId,
                PermissionId = p.Id,
                AssignedAt = DateTime.UtcNow,
                IsActive = true
            }).ToList();

            foreach (var rp in rolePermissionsToAdd)
            {
                await _rolePermissionRepository.AddAsync(rp, cancellationToken);
            }

            // Invalidate user permission caches for users assigned to this role
            if (_cacheService != null)
            {
                var affectedUserRoles = await _userRoleRepository.GetByUserIdAsync(request.RoleId, cancellationToken);
                foreach (var ur in affectedUserRoles)
                {
                    await _cacheService.RemoveAsync($"user_permissions_{ur.UserId}", cancellationToken);
                }
            }

            return new GrantRolePermissionResponseDTO
            {
                Success = true,
                GrantedCount = rolePermissionsToAdd.Count
            };
        }
    }
}
