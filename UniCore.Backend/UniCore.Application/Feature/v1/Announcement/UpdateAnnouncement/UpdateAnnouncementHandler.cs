using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.UpdateAnnouncement
{
    public class UpdateAnnouncementHandler : IRequestHandler<UpdateAnnouncementRequestDTO, UpdateAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementAudienceService _audienceService;
        private readonly IAnnouncementDeliveryService _deliveryService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateAnnouncementRequestDTO> _validator;

        public UpdateAnnouncementHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementAudienceService audienceService,
            IAnnouncementDeliveryService deliveryService,
            IMapper mapper,
            IValidator<UpdateAnnouncementRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _audienceService = audienceService;
            _deliveryService = deliveryService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UpdateAnnouncementResponseDTO> HandleAsync(UpdateAnnouncementRequestDTO request, CancellationToken cancellationToken)
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

            var currentStatus = AnnouncementLifecycle.ComputeStatus(entity.PublishDate, entity.ExpiredDate);
            if (!string.Equals(currentStatus, AnnouncementConstants.Status.Upcoming, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only UPCOMING announcements can be updated.");
            }

            var scopeType = request.ScopeType.Trim().ToUpperInvariant();
            var type = request.Type.Trim().ToUpperInvariant();
            if (scopeType == AnnouncementConstants.Scope.Public)
            {
                type = AnnouncementConstants.Type.Normal;
            }

            var scopeValue = AnnouncementScopeStorage.BuildScopeValueForRequest(
                scopeType,
                request.ScopeValue,
                request.Targets);
            AnnouncementScopeStorage.EnsureStoredLength(scopeValue);

            var targetStudentIds = ResolveTargetStudentIds(scopeType, request.TargetStudentIds, request.Targets);

            await _audienceService.ValidateScopeAsync(
                scopeType,
                scopeValue,
                targetStudentIds,
                cancellationToken);
            var now = DateTime.UtcNow;

            entity.Title = request.Title.Trim();
            entity.Description = request.Description;
            entity.Content = request.Content;
            entity.Type = type;
            entity.ScopeType = scopeType;
            entity.ScopeValue = scopeValue;
            entity.RequireAcknowledgement = request.RequireAcknowledgement;
            entity.PublishDate = request.PublishDate;
            entity.ExpiredDate = request.ExpiredDate;
            entity.Status = AnnouncementLifecycle.ComputeStatus(request.PublishDate, request.ExpiredDate, now);
            entity.UpdatedAt = now;
            entity.UpdatedBy = request.ActorUserId;

            await _deliveryService.ApplyAudienceSideEffectsAsync(
                entity,
                targetStudentIds,
                cancellationToken);

            await _announcementRepository.UpdateAsync(entity, cancellationToken);

            var updated = await _announcementRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);

            return new UpdateAnnouncementResponseDTO
            {
                Announcement = _mapper.Map<AnnouncementDTO>(updated)
            };
        }

        private static List<string> ResolveTargetStudentIds(
            string scopeType,
            List<string> targetStudentIds,
            List<string> targets)
        {
            if (string.Equals(scopeType, AnnouncementConstants.Scope.SpecificStudents, StringComparison.OrdinalIgnoreCase)
                && targetStudentIds.Count == 0
                && targets.Count > 0)
            {
                return targets;
            }

            return targetStudentIds;
        }
    }
}
