using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.UpdateAuditLog
{
    public class UpdateAuditLogValidator : AbstractValidator<UpdateAuditLogRequestDTO>
    {
        public UpdateAuditLogValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Audit log ID is required.");

            RuleFor(x => x.LogLevel)
                .NotEmpty().WithMessage("LogLevel is required.")
                .MaximumLength(50).WithMessage("LogLevel must not exceed 50 characters.");

            When(x => !string.IsNullOrWhiteSpace(x.Thread), () =>
            {
                RuleFor(x => x.Thread)
                    .MaximumLength(255).WithMessage("Thread must not exceed 255 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Logger), () =>
            {
                RuleFor(x => x.Logger)
                    .MaximumLength(255).WithMessage("Logger must not exceed 255 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.MachineName), () =>
            {
                RuleFor(x => x.MachineName)
                    .MaximumLength(255).WithMessage("MachineName must not exceed 255 characters.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.TraceId), () =>
            {
                RuleFor(x => x.TraceId)
                    .MaximumLength(255).WithMessage("TraceId must not exceed 255 characters.");
            });
        }
    }
}
