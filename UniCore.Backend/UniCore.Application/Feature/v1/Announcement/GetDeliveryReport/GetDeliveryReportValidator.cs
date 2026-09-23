using FluentValidation;

namespace UniCore.Application.Feature.v1.Announcement.GetDeliveryReport
{
    public class GetDeliveryReportValidator : AbstractValidator<GetDeliveryReportRequestDTO>
    {
        public GetDeliveryReportValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Announcement ID is required.");
        }
    }
}
