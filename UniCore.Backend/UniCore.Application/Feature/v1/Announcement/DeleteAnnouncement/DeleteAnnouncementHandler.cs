using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.DeleteAnnouncement
{
    public class DeleteAnnouncementHandler : IRequestHandler<DeleteAnnouncementRequestDTO, DeleteAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IValidator<DeleteAnnouncementRequestDTO> _validator;

        public DeleteAnnouncementHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            IValidator<DeleteAnnouncementRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _validator = validator;
        }

        public async Task<DeleteAnnouncementResponseDTO> HandleAsync(DeleteAnnouncementRequestDTO request, CancellationToken cancellationToken)
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

            var status = AnnouncementLifecycle.ComputeStatus(entity.PublishDate, entity.ExpiredDate);
            if (!string.Equals(status, AnnouncementConstants.Status.Upcoming, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only UPCOMING announcements can be deleted.");
            }

            await _announcementStudentRepository.ReplaceRecipientsAsync(entity.Id, Array.Empty<string>(), cancellationToken);
            await _announcementRepository.DeleteAsync(entity, cancellationToken);

            return new DeleteAnnouncementResponseDTO { Success = true };
        }
    }
}
