using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.DeleteRole
{
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleRequestDTO, DeleteRoleResponseDTO>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<DeleteRoleRequestDTO> _validator;

        public DeleteRoleHandler(IRoleRepository roleRepository, IValidator<DeleteRoleRequestDTO> validator)
        {
            _roleRepository = roleRepository;
            _validator = validator;
        }

        public async Task<DeleteRoleResponseDTO> HandleAsync(DeleteRoleRequestDTO request, CancellationToken cancellationToken)
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

            await _roleRepository.DeleteAsync(role, cancellationToken);

            return new DeleteRoleResponseDTO { Success = true };
        }
    }
}
