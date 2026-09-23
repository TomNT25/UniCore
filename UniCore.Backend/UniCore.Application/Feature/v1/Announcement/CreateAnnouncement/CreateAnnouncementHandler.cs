using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.CreateAnnouncement
{
    public class CreateAnnouncementHandler : IRequestHandler<CreateAnnouncementRequestDTO, CreateAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementAudienceService _audienceService;
        private readonly IAnnouncementDeliveryService _deliveryService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAnnouncementRequestDTO> _validator;

        public CreateAnnouncementHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementAudienceService audienceService,
            IAnnouncementDeliveryService deliveryService,
            IMapper mapper,
            IValidator<CreateAnnouncementRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _audienceService = audienceService;
            _deliveryService = deliveryService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CreateAnnouncementResponseDTO> HandleAsync(CreateAnnouncementRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var scopeType = request.ScopeType.Trim().ToUpperInvariant();
            var type = request.Type.Trim().ToUpperInvariant();

            // PUBLIC announcements are always NORMAL (no read-tracking / email).
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
            var status = AnnouncementLifecycle.ComputeStatus(request.PublishDate, request.ExpiredDate, now);

            var entity = new UniCore.Application.Entity.Announcement
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title.Trim(),
                Description = request.Description,
                Content = request.Content,
                Type = type,
                Status = status,
                ScopeType = scopeType,
                ScopeValue = scopeValue,
                RequireAcknowledgement = request.RequireAcknowledgement,
                PublishDate = request.PublishDate,
                ExpiredDate = request.ExpiredDate,
                CreatedAt = now,
                CreatedBy = request.ActorUserId
            };

            await _announcementRepository.AddAsync(entity, cancellationToken);

            await _deliveryService.ApplyAudienceSideEffectsAsync(
                entity,
                targetStudentIds,
                cancellationToken);

            await _announcementRepository.UpdateAsync(entity, cancellationToken);

            var created = await _announcementRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);

            return new CreateAnnouncementResponseDTO
            {
                Announcement = _mapper.Map<AnnouncementDTO>(created)
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
