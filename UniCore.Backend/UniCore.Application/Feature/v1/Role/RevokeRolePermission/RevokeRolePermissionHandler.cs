using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.RevokeRolePermission
{
    public class RevokeRolePermissionHandler : IRequestHandler<RevokeRolePermissionRequestDTO, RevokeRolePermissionResponseDTO>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ICacheService? _cacheService;
        private readonly IValidator<RevokeRolePermissionRequestDTO> _validator;

        public RevokeRolePermissionHandler(
            IRolePermissionRepository rolePermissionRepository,
            IUserRoleRepository userRoleRepository,
            IValidator<RevokeRolePermissionRequestDTO> validator,
            ICacheService? cacheService = null)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _userRoleRepository = userRoleRepository;
            _validator = validator;
            _cacheService = cacheService;
        }

        public async Task<RevokeRolePermissionResponseDTO> HandleAsync(RevokeRolePermissionRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var toRemove = await _rolePermissionRepository.GetByRoleAndPermissionIdsAsync(request.RoleId, request.PermissionIds, cancellationToken);
            if (!toRemove.Any())
            {
                return new RevokeRolePermissionResponseDTO { Success = true, RevokedCount = 0 };
            }

            foreach (var rp in toRemove)
            {
                await _rolePermissionRepository.DeleteAsync(rp, cancellationToken);
            }

            // Invalidate cache
            if (_cacheService != null)
            {
                var affectedUserRoles = await _userRoleRepository.GetByUserIdAsync(request.RoleId, cancellationToken);
                foreach (var ur in affectedUserRoles)
                {
                    await _cacheService.RemoveAsync($"user_permissions_{ur.UserId}", cancellationToken);
                }
            }

            return new RevokeRolePermissionResponseDTO
            {
                Success = true,
                RevokedCount = toRemove.Count
            };
        }
    }
}
