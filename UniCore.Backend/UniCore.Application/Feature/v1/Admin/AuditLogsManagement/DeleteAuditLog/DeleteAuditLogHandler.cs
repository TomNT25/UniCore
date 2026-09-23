using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.DeleteAuditLog
{
    public class DeleteAuditLogHandler : IRequestHandler<DeleteAuditLogRequestDTO, DeleteAuditLogResponseDTO>
    {
        private readonly IAppLogRepository _appLogRepository;
        private readonly IValidator<DeleteAuditLogRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;

        public DeleteAuditLogHandler(
            IAppLogRepository appLogRepository,
            IValidator<DeleteAuditLogRequestDTO> validator,
            IJsonStringLocalizer localizer)
        {
            _appLogRepository = appLogRepository;
            _validator = validator;
            _localizer = localizer;
        }

        public async Task<DeleteAuditLogResponseDTO> HandleAsync(DeleteAuditLogRequestDTO request, CancellationToken cancellationToken)
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

            var success = await _appLogRepository.DeleteAsync(logEntity, cancellationToken);

            return new DeleteAuditLogResponseDTO
            {
                Success = success
            };
        }
    }
}
