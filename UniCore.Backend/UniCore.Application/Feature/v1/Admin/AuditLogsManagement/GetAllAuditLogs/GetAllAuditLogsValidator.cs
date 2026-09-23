using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs
{
    public class GetAllAuditLogsValidator : AbstractValidator<GetAllAuditLogsRequestDTO>
    {
        public GetAllAuditLogsValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal to 1.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");

            When(x => !string.IsNullOrWhiteSpace(x.LogLevel), () =>
            {
                RuleFor(x => x.LogLevel)
                    .MaximumLength(50).WithMessage("LogLevel must not exceed 50 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.TraceId), () =>
            {
                RuleFor(x => x.TraceId)
                    .MaximumLength(255).WithMessage("TraceId must not exceed 255 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.SearchTerm), () =>
            {
                RuleFor(x => x.SearchTerm)
                    .MaximumLength(100).WithMessage("SearchTerm must not exceed 100 characters.");
            });

            When(x => x.FromDate.HasValue && x.ToDate.HasValue, () =>
            {
                RuleFor(x => x.FromDate)
                    .LessThanOrEqualTo(x => x.ToDate)
                    .WithMessage("FromDate must be earlier than or equal to ToDate.");
            });
        }
    }
}
