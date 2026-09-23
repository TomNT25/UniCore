using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.GetDeliveryReport
{
    public class GetDeliveryReportHandler : IRequestHandler<GetDeliveryReportRequestDTO, GetDeliveryReportResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IValidator<GetDeliveryReportRequestDTO> _validator;

        public GetDeliveryReportHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            IValidator<GetDeliveryReportRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _validator = validator;
        }

        public async Task<GetDeliveryReportResponseDTO> HandleAsync(GetDeliveryReportRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _announcementRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {request.Id} not found.");
            }

            var (total, viewed, acknowledged) = await _announcementStudentRepository.GetDeliveryStatsAsync(
                entity.Id,
                cancellationToken);

            if (total == 0
                && !AnnouncementConstants.Type.RequiresSideEffects(entity.Type)
                && !string.Equals(entity.ScopeType, AnnouncementConstants.Scope.SpecificStudents, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Delivery report is only available for announcements with recipient tracking (IMPORTANT/URGENT or SPECIFIC_STUDENTS).");
            }

            var recipients = await _announcementStudentRepository.GetByAnnouncementIdAsync(entity.Id, cancellationToken);
            var status = AnnouncementLifecycle.ComputeStatus(entity.PublishDate, entity.ExpiredDate);

            return new GetDeliveryReportResponseDTO
            {
                AnnouncementId = entity.Id,
                Title = entity.Title,
                Status = status,
                RecipientCount = total,
                ViewedCount = viewed,
                AcknowledgedCount = acknowledged,
                Recipients = recipients.Select(r => new DeliveryRecipientDTO
                {
                    StudentId = r.StudentId,
                    Username = r.Student?.Username,
                    Email = r.Student?.Email,
                    ViewedAt = r.ViewedAt,
                    AcknowledgedAt = r.AcknowledgedAt
                }).ToList()
            };
        }
    }
}
