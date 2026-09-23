using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Permission.GetPermissionById
{
    public class GetPermissionByIdHandler : IRequestHandler<GetPermissionByIdRequestDTO, GetPermissionByIdResponseDTO>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetPermissionByIdRequestDTO> _validator;

        public GetPermissionByIdHandler(
            IPermissionRepository permissionRepository,
            IMapper mapper,
            IValidator<GetPermissionByIdRequestDTO> validator)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetPermissionByIdResponseDTO> HandleAsync(GetPermissionByIdRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var permission = await _permissionRepository.GetByIdAsync(request.Id, cancellationToken);

            return new GetPermissionByIdResponseDTO
            {
                Permission = _mapper.Map<PermissionDTO>(permission)
            };
        }
    }
}
