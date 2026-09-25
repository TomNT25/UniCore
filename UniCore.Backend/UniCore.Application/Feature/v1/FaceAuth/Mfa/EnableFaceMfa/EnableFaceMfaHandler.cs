using FluentValidation;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.EnableFaceMfa
{
    public class EnableFaceMfaHandler : IRequestHandler<EnableFaceMfaRequestDTO, EnableFaceMfaResponseDTO>
    {
        private readonly IUserFaceProfileRepository _faceProfileRepository;
        private readonly IUserMfaSettingRepository _mfaSettingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFaceAuthAuditService _auditService;
        private readonly IValidator<EnableFaceMfaRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;
        private readonly ILogger<EnableFaceMfaHandler> _logger;

        public EnableFaceMfaHandler(
            IUserFaceProfileRepository faceProfileRepository,
            IUserMfaSettingRepository mfaSettingRepository,
            IUserRepository userRepository,
            IFaceAuthAuditService auditService,
            IValidator<EnableFaceMfaRequestDTO> validator,
            IJsonStringLocalizer localizer,
            ILogger<EnableFaceMfaHandler> logger)
        {
            _faceProfileRepository = faceProfileRepository;
            _mfaSettingRepository = mfaSettingRepository;
            _userRepository = userRepository;
            _auditService = auditService;
            _validator = validator;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<EnableFaceMfaResponseDTO> HandleAsync(
            EnableFaceMfaRequestDTO request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("User not found or inactive for Face MFA enablement: {UserId}", request.UserId);
                return new EnableFaceMfaResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.AccountSuspended,
                    ErrorMessage = "User account not found or inactive."
                };
            }

            // Prerequisite: User must have enrolled face in AI Service
            var faceProfile = await _faceProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (faceProfile == null ||
                faceProfile.Status == FaceAuthConstants.Status.NotEnrolled)
            {
                _logger.LogWarning("Face MFA enablement rejected: User {UserId} has not enrolled face biometrics", request.UserId);
                return new EnableFaceMfaResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NotEnrolled,
                    ErrorMessage = _localizer.GetString(MessageConstants.FaceAuth.EnrollmentRequiredForMfa)
                };
            }

            var now = DateTime.UtcNow;

            // Upsert UserMfaSetting
            var mfaSettingList = await _mfaSettingRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            var mfaSetting = mfaSettingList.FirstOrDefault(s => s != null && s.IsActive && s.MfaMethod == "FACE");
            if (mfaSetting == null)
            {
                mfaSetting = new UserMfaSetting
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    MfaMethod = "FACE",
                    IsMfaEnabled = true,
                    EnabledAt = now,
                    IsActive = true,
                    CreatedAt = now,
                    CreatedBy = request.UserId
                };
                await _mfaSettingRepository.AddAsync(mfaSetting, cancellationToken);
            }
            else
            {
                mfaSetting.MfaMethod = "FACE";
                mfaSetting.IsMfaEnabled = true;
                mfaSetting.EnabledAt = now;
                mfaSetting.UpdatedAt = now;
                mfaSetting.UpdatedBy = request.UserId;
                await _mfaSettingRepository.UpdateAsync(mfaSetting, cancellationToken);
            }

            // Ensure face profile status is ENROLLED
            if (faceProfile.Status == FaceAuthConstants.Status.PendingPin)
            {
                faceProfile.Status = FaceAuthConstants.Status.Enrolled;
                faceProfile.UpdatedAt = now;
                faceProfile.UpdatedBy = request.UserId;
                await _faceProfileRepository.UpdateAsync(faceProfile, cancellationToken);
            }

            await _auditService.LogAsync(new FaceAuthAuditEntry
            {
                UserId = request.UserId,
                Action = FaceAuthAction.EnableMfa,
                Result = FaceAuthResult.Success
            }, cancellationToken);

            _logger.LogInformation("Face Recognition MFA enabled successfully for user {UserId}", request.UserId);

            return new EnableFaceMfaResponseDTO
            {
                Success = true,
                Message = _localizer.GetString(MessageConstants.FaceAuth.EnableMfaSuccess),
                MfaMethod = "FACE",
                EnabledAt = now
            };
        }
    }
}
