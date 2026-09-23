using FluentValidation;

namespace UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAuditLogById
{
    public class GetAuditLogByIdValidator : AbstractValidator<GetAuditLogByIdRequestDTO>
    {
        public GetAuditLogByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Audit log ID is required.");
        }
    }
}
