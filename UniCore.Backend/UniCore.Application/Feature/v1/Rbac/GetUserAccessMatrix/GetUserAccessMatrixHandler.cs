using FluentValidation;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Rbac.GetUserAccessMatrix
{
    public class GetUserAccessMatrixHandler : IRequestHandler<GetUserAccessMatrixRequestDTO, GetUserAccessMatrixResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetUserAccessMatrixRequestDTO> _validator;

        public GetUserAccessMatrixHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IUserRoleRepository userRoleRepository,
            IUserPermissionRepository userPermissionRepository,
            IRolePermissionRepository rolePermissionRepository,
            IMapper mapper,
            IValidator<GetUserAccessMatrixRequestDTO> validator)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _userPermissionRepository = userPermissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetUserAccessMatrixResponseDTO> HandleAsync(GetUserAccessMatrixRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return new GetUserAccessMatrixResponseDTO();
            }

            var allRoles = await _roleRepository.GetAllAsync(cancellationToken);
            var allPermissions = await _permissionRepository.GetAllAsync(cancellationToken);
            var allRolePermissions = await _rolePermissionRepository.GetAllAsync(cancellationToken);
            var userRoles = await _userRoleRepository.GetAllAsync(cancellationToken);
            var userPermissions = await _userPermissionRepository.GetAllAsync(cancellationToken);

            var permissionsDict = allPermissions.ToDictionary(p => p.Id);
            var rolesDict = allRoles.ToDictionary(r => r.Id);

            var roleDetailsList = new List<UserRoleDetailDTO>();
            var effectivePermissionsDict = new Dictionary<string, UniCore.Application.Entity.Permission>();

            // Additional User Roles
            var assignedUserRoles = userRoles.Where(ur => ur.UserId == user.Id && ur.IsActive).ToList();
            foreach (var ur in assignedUserRoles)
            {
                if (rolesDict.TryGetValue(ur.RoleId, out var addRole))
                {
                    var rolePermIds = allRolePermissions
                        .Where(rp => rp.RoleId == addRole.Id && rp.IsActive)
                        .Select(rp => rp.PermissionId)
                        .Distinct();

                    var rolePerms = rolePermIds
                        .Where(id => permissionsDict.ContainsKey(id))
                        .Select(id => permissionsDict[id])
                        .ToList();

                    foreach (var p in rolePerms)
                    {
                        effectivePermissionsDict[p.Id] = p;
                    }

                    roleDetailsList.Add(new UserRoleDetailDTO
                    {
                        RoleId = addRole.Id,
                        RoleCode = addRole.Code,
                        RoleName = addRole.Name,
                        AssignmentType = "Additional",
                        RolePermissions = rolePerms.Select(p => _mapper.Map<PermissionDTO>(p)).ToList()
                    });
                }
            }

            // Direct User Permissions
            var directPermIds = userPermissions
                .Where(up => up.UserId == user.Id && up.IsActive)
                .Select(up => up.PermissionId)
                .Distinct();

            var directPerms = directPermIds
                .Where(id => permissionsDict.ContainsKey(id))
                .Select(id => permissionsDict[id])
                .ToList();

            foreach (var p in directPerms)
            {
                effectivePermissionsDict[p.Id] = p;
            }

            return new GetUserAccessMatrixResponseDTO
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = roleDetailsList,
                DirectUserPermissions = directPerms.Select(p => _mapper.Map<PermissionDTO>(p)).ToList(),
                EffectivePermissions = effectivePermissionsDict.Values.Select(p => _mapper.Map<PermissionDTO>(p)).ToList()
            };
        }
    }
}
