using FluentValidation;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.DisableFaceMfa
{
    public class DisableFaceMfaHandler : IRequestHandler<DisableFaceMfaRequestDTO, DisableFaceMfaResponseDTO>
    {
        private readonly IUserMfaSettingRepository _mfaSettingRepository;
        private readonly IFaceAuthAuditService _auditService;
        private readonly IValidator<DisableFaceMfaRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;
        private readonly ILogger<DisableFaceMfaHandler> _logger;

        public DisableFaceMfaHandler(
            IUserMfaSettingRepository mfaSettingRepository,
            IFaceAuthAuditService auditService,
            IValidator<DisableFaceMfaRequestDTO> validator,
            IJsonStringLocalizer localizer,
            ILogger<DisableFaceMfaHandler> logger)
        {
            _mfaSettingRepository = mfaSettingRepository;
            _auditService = auditService;
            _validator = validator;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<DisableFaceMfaResponseDTO> HandleAsync(
            DisableFaceMfaRequestDTO request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var mfaSettingList = await _mfaSettingRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            var faceMfaSetting = mfaSettingList.FirstOrDefault(s => s != null && s.IsActive && s.MfaMethod == "FACE");
            if (faceMfaSetting != null)
            {
                faceMfaSetting.IsMfaEnabled = false;
                faceMfaSetting.UpdatedAt = DateTime.UtcNow;
                faceMfaSetting.UpdatedBy = request.UserId;
                await _mfaSettingRepository.UpdateAsync(faceMfaSetting, cancellationToken);
            }

            await _auditService.LogAsync(new FaceAuthAuditEntry
            {
                UserId = request.UserId,
                Action = FaceAuthAction.DisableMfa,
                Result = FaceAuthResult.Success
            }, cancellationToken);

            _logger.LogInformation("Face Recognition MFA disabled for user {UserId}", request.UserId);

            return new DisableFaceMfaResponseDTO
            {
                Success = true,
                Message = _localizer.GetString(MessageConstants.FaceAuth.DisableMfaSuccess)
            };
        }
    }
}
