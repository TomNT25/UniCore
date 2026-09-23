using FluentValidation;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Rbac.GetRbacOverview
{
    public class GetRbacOverviewHandler : IRequestHandler<GetRbacOverviewRequestDTO, GetRbacOverviewResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IValidator<GetRbacOverviewRequestDTO> _validator;

        public GetRbacOverviewHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IUserRoleRepository userRoleRepository,
            IUserPermissionRepository userPermissionRepository,
            IRolePermissionRepository rolePermissionRepository,
            IValidator<GetRbacOverviewRequestDTO> validator)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _userPermissionRepository = userPermissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _validator = validator;
        }

        public async Task<GetRbacOverviewResponseDTO> HandleAsync(GetRbacOverviewRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var totalUsers = await _userRepository.CountAsync(null, cancellationToken);
            var totalRoles = await _roleRepository.CountAsync(null, cancellationToken);
            var totalPermissions = await _permissionRepository.CountAsync(null, cancellationToken);
            var totalUserRoles = await _userRoleRepository.CountAsync(null, cancellationToken);
            var totalUserPermissions = await _userPermissionRepository.CountAsync(null, cancellationToken);
            var totalRolePermissions = await _rolePermissionRepository.CountAsync(null, cancellationToken);

            var roles = await _roleRepository.GetAllAsync(cancellationToken);
            var rolePermissions = await _rolePermissionRepository.GetAllAsync(cancellationToken);
            var userRoles = await _userRoleRepository.GetAllAsync(cancellationToken);
            var users = await _userRepository.GetAllAsync(cancellationToken);

            var rolesOverview = roles.Select(role =>
            {
                var assignedPermissionsCount = rolePermissions.Count(rp => rp.RoleId == role.Id);
                var assignedUsersCount = userRoles.Count(ur => ur.RoleId == role.Id);

                return new RoleOverviewDTO
                {
                    Id = role.Id,
                    Code = role.Code,
                    Name = role.Name,
                    Description = role.Description ?? string.Empty,
                    IsActive = role.IsActive,
                    AssignedPermissionsCount = assignedPermissionsCount,
                    AssignedUsersCount = assignedUsersCount
                };
            }).ToList();

            return new GetRbacOverviewResponseDTO
            {
                TotalUsers = totalUsers,
                TotalRoles = totalRoles,
                TotalPermissions = totalPermissions,
                TotalUserRoles = totalUserRoles,
                TotalUserPermissions = totalUserPermissions,
                TotalRolePermissions = totalRolePermissions,
                RolesOverview = rolesOverview
            };
        }
    }
}
