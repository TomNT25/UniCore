using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Entity;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs
{
    public class GetAllAuditLogsHandler : IRequestHandler<GetAllAuditLogsRequestDTO, GetAllAuditLogsResponseDTO>
    {
        private readonly IAppLogRepository _appLogRepository;
        private readonly IValidator<GetAllAuditLogsRequestDTO> _validator;

        public GetAllAuditLogsHandler(
            IAppLogRepository appLogRepository,
            IValidator<GetAllAuditLogsRequestDTO> validator)
        {
            _appLogRepository = appLogRepository;
            _validator = validator;
        }

        public async Task<GetAllAuditLogsResponseDTO> HandleAsync(GetAllAuditLogsRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            // Set default sorting to newest first if not explicitly specified
            if (string.IsNullOrWhiteSpace(request.SortColumn))
            {
                request.SortColumn = "LogDate";
                request.SortDescending = true;
            }

            Expression<Func<AppLog, bool>> filter = l =>
                (string.IsNullOrWhiteSpace(request.LogLevel) || l.LogLevel == request.LogLevel) &&
                (!request.FromDate.HasValue || l.LogDate >= request.FromDate.Value) &&
                (!request.ToDate.HasValue || l.LogDate <= request.ToDate.Value) &&
                (string.IsNullOrWhiteSpace(request.TraceId) || l.TraceId == request.TraceId) &&
                (string.IsNullOrWhiteSpace(request.SearchTerm) ||
                    (l.Message != null && l.Message.Contains(request.SearchTerm)) ||
                    (l.Logger != null && l.Logger.Contains(request.SearchTerm)) ||
                    (l.Exception != null && l.Exception.Contains(request.SearchTerm)) ||
                    (l.Thread != null && l.Thread.Contains(request.SearchTerm)));

            var pagedResult = await _appLogRepository.GetPageNumberPaginationAsync<GetAllAuditLogsDTO>(
                request,
                filter,
                cancellationToken);

            return new GetAllAuditLogsResponseDTO
            {
                Items = pagedResult.Items,
                Metadata = pagedResult.Metadata
            };
        }
    }
}
