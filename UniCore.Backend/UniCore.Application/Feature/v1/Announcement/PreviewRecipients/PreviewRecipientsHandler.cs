using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.PreviewRecipients
{
    public class PreviewRecipientsHandler : IRequestHandler<PreviewRecipientsRequestDTO, PreviewRecipientsResponseDTO>
    {
        private readonly IAnnouncementAudienceService _audienceService;
        private readonly IValidator<PreviewRecipientsRequestDTO> _validator;

        public PreviewRecipientsHandler(
            IAnnouncementAudienceService audienceService,
            IValidator<PreviewRecipientsRequestDTO> validator)
        {
            _audienceService = audienceService;
            _validator = validator;
        }

        public async Task<PreviewRecipientsResponseDTO> HandleAsync(PreviewRecipientsRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var scopeType = request.ScopeType.Trim().ToUpperInvariant();
            var scopeValue = AnnouncementScopeStorage.BuildScopeValueForRequest(
                scopeType,
                request.ScopeValue,
                request.Targets);
            var targetStudentIds = request.TargetStudentIds.Count > 0
                ? request.TargetStudentIds
                : (string.Equals(scopeType, AnnouncementConstants.Scope.SpecificStudents, StringComparison.OrdinalIgnoreCase)
                    ? request.Targets
                    : request.TargetStudentIds);

            if (scopeType == AnnouncementConstants.Scope.Public)
            {
                await _audienceService.ValidateScopeAsync(request.ScopeType, scopeValue, null, cancellationToken);
                return new PreviewRecipientsResponseDTO
                {
                    RecipientCount = null,
                    IsPublicUnlimited = true,
                    StudentIds = new List<string>()
                };
            }

            var studentIds = await _audienceService.ResolveStudentIdsAsync(
                request.ScopeType,
                scopeValue,
                targetStudentIds,
                cancellationToken);

            return new PreviewRecipientsResponseDTO
            {
                RecipientCount = studentIds.Count,
                IsPublicUnlimited = false,
                StudentIds = studentIds.ToList()
            };
        }
    }
}
