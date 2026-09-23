using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.DeletePermission
{
    public class DeletePermissionHandler : IRequestHandler<DeletePermissionRequestDTO, DeletePermissionResponseDTO>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IValidator<DeletePermissionRequestDTO> _validator;

        public DeletePermissionHandler(
            IPermissionRepository permissionRepository,
            IValidator<DeletePermissionRequestDTO> validator)
        {
            _permissionRepository = permissionRepository;
            _validator = validator;
        }

        public async Task<DeletePermissionResponseDTO> HandleAsync(DeletePermissionRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _permissionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Permission with ID {request.Id} not found.");
            }

            await _permissionRepository.DeleteAsync(entity, cancellationToken);

            return new DeletePermissionResponseDTO { Success = true };
        }
    }
}
