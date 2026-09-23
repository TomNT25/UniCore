using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.DeleteAuditLog
{
    public class DeleteAuditLogValidator : AbstractValidator<DeleteAuditLogRequestDTO>
    {
        public DeleteAuditLogValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Audit log ID is required.");
        }
    }
}
