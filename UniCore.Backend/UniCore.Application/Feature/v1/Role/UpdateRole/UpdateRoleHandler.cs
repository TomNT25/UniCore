using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Role.UpdateRole
{
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleRequestDTO, UpdateRoleResponseDTO>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateRoleRequestDTO> _validator;

        public UpdateRoleHandler(
            IRoleRepository roleRepository,
            IMapper mapper,
            IValidator<UpdateRoleRequestDTO> validator)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UpdateRoleResponseDTO> HandleAsync(UpdateRoleRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.Id} not found.");
            }

            role.Name = request.Name;
            role.Description = request.Description;
            role.IsActive = request.IsActive;
            role.UpdatedAt = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(role, cancellationToken);

            return new UpdateRoleResponseDTO
            {
                Role = _mapper.Map<RoleDTO>(role)
            };
        }
    }
}
