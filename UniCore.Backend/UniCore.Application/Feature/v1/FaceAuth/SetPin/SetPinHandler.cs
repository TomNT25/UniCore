using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.SetPin
{
    public class SetPinHandler : IRequestHandler<SetPinRequestDTO, SetPinResponseDTO>
    {
        private readonly IUserFaceProfileRepository _faceProfileRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly ILogger<SetPinHandler> _logger;

        public SetPinHandler(
            IUserFaceProfileRepository faceProfileRepository,
            IPasswordHasherService passwordHasher,
            ILogger<SetPinHandler> logger)
        {
            _faceProfileRepository = faceProfileRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<SetPinResponseDTO> HandleAsync(
            SetPinRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing PIN setup for user {UserId}", request.UserId);

            // Get existing face profile
            var profile = await _faceProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (profile == null)
            {
                _logger.LogWarning("Face profile not found for user {UserId}", request.UserId);
                return new SetPinResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NotEnrolled,
                    ErrorMessage = "Face profile not found. Please enroll face first."
                };
            }

            // Check if account is suspended
            if (profile.Status == FaceAuthConstants.Status.Suspended)
            {
                _logger.LogWarning("Face profile is suspended for user {UserId}", request.UserId);
                return new SetPinResponseDTO
                {
                    Success = false,
                    Status = profile.Status,
                    ErrorCode = FaceAuthConstants.ErrorCodes.AccountSuspended,
                    ErrorMessage = "Face profile is suspended"
                };
            }

            // Check if face is enrolled (status should be PENDING_PIN or already ENROLLED for PIN change)
            if (profile.Status != FaceAuthConstants.Status.PendingPin &&
                profile.Status != FaceAuthConstants.Status.Enrolled)
            {
                _logger.LogWarning(
                    "Invalid status for PIN setup. User: {UserId}, Status: {Status}",
                    request.UserId, profile.Status);
                return new SetPinResponseDTO
                {
                    Success = false,
                    Status = profile.Status,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NotEnrolled,
                    ErrorMessage = "Face enrollment required before setting PIN"
                };
            }

            // Hash the PIN
            var pinHash = _passwordHasher.HashPassword(request.Pin);

            // Update profile with PIN
            var updatedProfile = await _faceProfileRepository.SetPinAsync(
                request.UserId,
                pinHash,
                request.UserId,
                cancellationToken);

            _logger.LogInformation(
                "PIN set successfully for user {UserId}. New status: {Status}",
                request.UserId, updatedProfile.Status);

            return new SetPinResponseDTO
            {
                Success = true,
                Status = updatedProfile.Status
            };
        }
    }
}
