using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.RevokeUserRole
{
    public class RevokeUserRoleHandler : IRequestHandler<RevokeUserRoleRequestDTO, RevokeUserRoleResponseDTO>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ICacheService? _cacheService;
        private readonly IValidator<RevokeUserRoleRequestDTO> _validator;

        public RevokeUserRoleHandler(
            IUserRoleRepository userRoleRepository,
            IValidator<RevokeUserRoleRequestDTO> validator,
            ICacheService? cacheService = null)
        {
            _userRoleRepository = userRoleRepository;
            _validator = validator;
            _cacheService = cacheService;
        }

        public async Task<RevokeUserRoleResponseDTO> HandleAsync(RevokeUserRoleRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var toRemove = await _userRoleRepository.GetByUserAndRoleIdsAsync(request.UserId, request.RoleIds, cancellationToken);
            if (!toRemove.Any())
            {
                return new RevokeUserRoleResponseDTO { Success = true, RevokedCount = 0 };
            }

            foreach (var ur in toRemove)
            {
                await _userRoleRepository.DeleteAsync(ur, cancellationToken);
            }

            if (_cacheService != null)
            {
                await _cacheService.RemoveAsync($"user_permissions_{request.UserId}", cancellationToken);
            }

            return new RevokeUserRoleResponseDTO
            {
                Success = true,
                RevokedCount = toRemove.Count
            };
        }
    }
}
