using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.UpdateAuditLog
{
    public class UpdateAuditLogHandler : IRequestHandler<UpdateAuditLogRequestDTO, UpdateAuditLogResponseDTO>
    {
        private readonly IAppLogRepository _appLogRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateAuditLogRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public UpdateAuditLogHandler(
            IAppLogRepository appLogRepository,
            IMapper mapper,
            IValidator<UpdateAuditLogRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _appLogRepository = appLogRepository;
            _mapper = mapper;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<UpdateAuditLogResponseDTO> HandleAsync(UpdateAuditLogRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var logEntity = await _appLogRepository.GetByIdAsync(request.Id, cancellationToken);
            if (logEntity == null)
            {
                var msg = _localizer.GetString(MessageConstants.Admin.AuditLogNotFound);
                throw new ValidationException(new[] { new ValidationFailure("Id", msg) });
            }

            logEntity.LogLevel = request.LogLevel.Trim().ToUpperInvariant();
            logEntity.Thread = string.IsNullOrWhiteSpace(request.Thread) ? null : request.Thread.Trim();
            logEntity.Logger = string.IsNullOrWhiteSpace(request.Logger) ? null : request.Logger.Trim();
            logEntity.Message = request.Message;
            logEntity.Exception = request.Exception;
            logEntity.MachineName = string.IsNullOrWhiteSpace(request.MachineName) ? logEntity.MachineName : request.MachineName.Trim();
            logEntity.TraceId = string.IsNullOrWhiteSpace(request.TraceId) ? null : request.TraceId.Trim();

            await _appLogRepository.UpdateAsync(logEntity, cancellationToken);

            return new UpdateAuditLogResponseDTO
            {
                Log = _mapper.Map<AppLogDTO>(logEntity)
            };
        }
    }
}
