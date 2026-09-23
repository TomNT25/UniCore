using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement;

namespace UniCore.Application.Feature.v1.Announcement.GetAnnouncementById
{
    public class GetAnnouncementByIdHandler : IRequestHandler<GetAnnouncementByIdRequestDTO, GetAnnouncementByIdResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IAnnouncementEmailLogRepository _emailLogRepository;
        private readonly IValidator<GetAnnouncementByIdRequestDTO> _validator;

        public GetAnnouncementByIdHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            IAnnouncementEmailLogRepository emailLogRepository,
            IValidator<GetAnnouncementByIdRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _emailLogRepository = emailLogRepository;
            _validator = validator;
        }

        public async Task<GetAnnouncementByIdResponseDTO> HandleAsync(GetAnnouncementByIdRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _announcementRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return new GetAnnouncementByIdResponseDTO { Announcement = null };
            }

            var utcNow = DateTime.UtcNow;
            var item = AdminAnnouncementMapping.ToAdminItem(entity, utcNow, includeContent: true);

            var recipients = await _announcementStudentRepository.GetByAnnouncementIdAsync(entity.Id, cancellationToken);
            var emailLogs = await _emailLogRepository.GetByAnnouncementIdAsync(entity.Id, cancellationToken);

            return new GetAnnouncementByIdResponseDTO
            {
                Announcement = item,
                Targets = recipients.Select(r => new AdminAnnouncementTargetDto
                {
                    Id = r.StudentId,
                    Name = ResolveStudentName(r.Student),
                    PersonalEmail = r.Student?.Email ?? string.Empty
                }).ToList(),
                SentEmail = emailLogs.Select(l => new AdminAnnouncementSentEmailDto
                {
                    SentDate = l.SentAt,
                    Status = l.Status
                }).ToList()
            };
        }

        private static string ResolveStudentName(UniCore.Application.Entity.User? student)
        {
            if (student == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(student.UserProfile?.FullName))
            {
                return student.UserProfile.FullName;
            }

            return student.Username;
        }
    }
}
