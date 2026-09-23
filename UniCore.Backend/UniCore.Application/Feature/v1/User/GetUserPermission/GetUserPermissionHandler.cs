using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.User.GetUserPermission
{
    public class GetUserPermissionHandler : IRequestHandler<GetUserPermissionRequestDTO, GetUserPermissionResponseDTO>
    {
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetUserPermissionRequestDTO> _validator;
        private readonly ICacheService? _cacheService;

        public GetUserPermissionHandler(
            IUserPermissionRepository userPermissionRepository,
            IMapper mapper,
            IValidator<GetUserPermissionRequestDTO> validator,
            ICacheService? cacheService = null)
        {
            _userPermissionRepository = userPermissionRepository;
            _mapper = mapper;
            _validator = validator;
            _cacheService = cacheService;
        }

        public async Task<GetUserPermissionResponseDTO> HandleAsync(GetUserPermissionRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var cacheKey = $"user_permissions_{request.UserID}";
            var cacheDuration = TimeSpan.FromMinutes(15);

            if (_cacheService != null)
            {
                var cached = await _cacheService.GetAsync<GetUserPermissionResponseDTO>(cacheKey, cancellationToken);
                if (cached != null)
                {
                    return cached;
                }
            }

            var result = await _userPermissionRepository.GetByUserIDAsync(request.UserID, cancellationToken);

            var response = new GetUserPermissionResponseDTO()
            {
                Permissions = _mapper.Map<List<PermissionDTO>>(result.Select(x => x.Permission))
            };

            if (_cacheService != null)
            {
                await _cacheService.SetAsync(cacheKey, response, cacheDuration, cancellationToken);
            }

            return response;
        }
    }
}


