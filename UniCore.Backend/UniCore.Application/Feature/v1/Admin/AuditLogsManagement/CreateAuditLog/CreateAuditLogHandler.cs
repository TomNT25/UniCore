using FluentValidation;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.CreateAuditLog
{
    public class CreateAuditLogHandler : IRequestHandler<CreateAuditLogRequestDTO, CreateAuditLogResponseDTO>
    {
        private readonly IAppLogRepository _appLogRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAuditLogRequestDTO> _validator;

        public CreateAuditLogHandler(
            IAppLogRepository appLogRepository,
            IMapper mapper,
            IValidator<CreateAuditLogRequestDTO> validator)
        {
            _appLogRepository = appLogRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CreateAuditLogResponseDTO> HandleAsync(CreateAuditLogRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var logEntity = new AppLog
            {
                Id = Guid.NewGuid().ToString(),
                LogDate = DateTime.UtcNow,
                LogLevel = request.LogLevel.Trim().ToUpperInvariant(),
                Thread = string.IsNullOrWhiteSpace(request.Thread) ? null : request.Thread.Trim(),
                Logger = string.IsNullOrWhiteSpace(request.Logger) ? null : request.Logger.Trim(),
                Message = request.Message,
                Exception = request.Exception,
                MachineName = string.IsNullOrWhiteSpace(request.MachineName) ? Environment.MachineName : request.MachineName.Trim(),
                TraceId = string.IsNullOrWhiteSpace(request.TraceId) ? null : request.TraceId.Trim()
            };

            await _appLogRepository.AddAsync(logEntity, cancellationToken);

            return new CreateAuditLogResponseDTO
            {
                Log = _mapper.Map<AuditLogSummaryDTO>(logEntity)
            };
        }
    }
}
