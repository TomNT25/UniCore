using FluentValidation;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAuditLogById
{
    public class GetAuditLogByIdHandler : IRequestHandler<GetAuditLogByIdRequestDTO, GetAuditLogByIdResponseDTO>
    {
        private readonly IAppLogRepository _appLogRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAuditLogByIdRequestDTO> _validator;

        public GetAuditLogByIdHandler(
            IAppLogRepository appLogRepository,
            IMapper mapper,
            IValidator<GetAuditLogByIdRequestDTO> validator)
        {
            _appLogRepository = appLogRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetAuditLogByIdResponseDTO> HandleAsync(GetAuditLogByIdRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var logEntity = await _appLogRepository.GetByIdAsync(request.Id, cancellationToken);
            return new GetAuditLogByIdResponseDTO
            {
                Log = logEntity != null ? _mapper.Map<AuditLogDetailDTO>(logEntity) : null
            };
        }
    }
}
