using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Role.GetRoleById
{
    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdRequestDTO, GetRoleByIdResponseDTO>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetRoleByIdRequestDTO> _validator;

        public GetRoleByIdHandler(
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IMapper mapper,
            IValidator<GetRoleByIdRequestDTO> validator)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetRoleByIdResponseDTO> HandleAsync(GetRoleByIdRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (role == null)
            {
                return new GetRoleByIdResponseDTO();
            }

            var rolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(role.Id, cancellationToken);

            return new GetRoleByIdResponseDTO
            {
                Role = _mapper.Map<RoleDTO>(role),
                Permissions = _mapper.Map<List<PermissionDTO>>(rolePermissions.Select(rp => rp.Permission))
            };
        }
    }
}
