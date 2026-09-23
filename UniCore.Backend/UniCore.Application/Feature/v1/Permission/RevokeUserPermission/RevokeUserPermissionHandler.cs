using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.RevokeUserPermission
{
    public class RevokeUserPermissionHandler : IRequestHandler<RevokeUserPermissionRequestDTO, RevokeUserPermissionResponseDTO>
    {
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly ICacheService? _cacheService;
        private readonly IValidator<RevokeUserPermissionRequestDTO> _validator;

        public RevokeUserPermissionHandler(
            IUserPermissionRepository userPermissionRepository,
            IValidator<RevokeUserPermissionRequestDTO> validator,
            ICacheService? cacheService = null)
        {
            _userPermissionRepository = userPermissionRepository;
            _validator = validator;
            _cacheService = cacheService;
        }

        public async Task<RevokeUserPermissionResponseDTO> HandleAsync(RevokeUserPermissionRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var toRemove = await _userPermissionRepository.GetByUserAndPermissionIdsAsync(request.UserId, request.PermissionIds, cancellationToken);
            if (!toRemove.Any())
            {
                return new RevokeUserPermissionResponseDTO { Success = true, RevokedCount = 0 };
            }

            foreach (var up in toRemove)
            {
                await _userPermissionRepository.DeleteAsync(up, cancellationToken);
            }

            if (_cacheService != null)
            {
                await _cacheService.RemoveAsync($"user_permissions_{request.UserId}", cancellationToken);
            }

            return new RevokeUserPermissionResponseDTO
            {
                Success = true,
                RevokedCount = toRemove.Count
            };
        }
    }
}
