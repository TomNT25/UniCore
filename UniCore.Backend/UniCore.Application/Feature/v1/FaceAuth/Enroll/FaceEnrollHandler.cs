using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.Enroll
{
    public class FaceEnrollHandler : IRequestHandler<FaceEnrollRequestDTO, FaceEnrollResponseDTO>
    {
        private readonly IFaceAiClient _faceAiClient;
        private readonly IUserFaceProfileRepository _faceProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<FaceEnrollHandler> _logger;

        public FaceEnrollHandler(
            IFaceAiClient faceAiClient,
            IUserFaceProfileRepository faceProfileRepository,
            IUserRepository userRepository,
            ILogger<FaceEnrollHandler> logger)
        {
            _faceAiClient = faceAiClient;
            _faceProfileRepository = faceProfileRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<FaceEnrollResponseDTO> HandleAsync(
            FaceEnrollRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing face enrollment for user {UserId} with {ImageCount} images",
                request.UserId, request.FaceImages.Count);

            // Get user to obtain username
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", request.UserId);
                return new FaceEnrollResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                    ErrorMessage = "User not found"
                };
            }

            // Check if user is active
            if (!user.IsActive)
            {
                _logger.LogWarning("User is inactive: {UserId}", request.UserId);
                return new FaceEnrollResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.AccountSuspended,
                    ErrorMessage = "User account is inactive"
                };
            }

            // Get username from user entity (Username or Email as fallback)
            var username = !string.IsNullOrWhiteSpace(user.Username) ? user.Username : user.Email;

            // Convert to Face AI input format
            var faceImages = request.FaceImages
                .Select(img => new FaceImageInput
                {
                    Stream = img.Stream,
                    FileName = img.FileName,
                    ContentType = img.ContentType,
                    Length = img.Length
                })
                .ToList();

            // Call Face AI enroll
            var enrollResult = await _faceAiClient.EnrollAsync(
                request.UserId,
                username,
                faceImages,
                cancellationToken);

            if (!enrollResult.Success)
            {
                _logger.LogWarning(
                    "Face AI enrollment failed for user {UserId}: {ErrorCode} - {ErrorMessage}",
                    request.UserId, enrollResult.ErrorCode, enrollResult.ErrorMessage);

                return new FaceEnrollResponseDTO
                {
                    Success = false,
                    ErrorCode = enrollResult.ErrorCode,
                    ErrorMessage = enrollResult.ErrorMessage
                };
            }

            // Save to database
            var profile = await _faceProfileRepository.UpsertEnrollmentAsync(
                request.UserId,
                enrollResult.EmbeddingId!,
                enrollResult.ModelVersion ?? "unknown",
                request.UserId,
                cancellationToken);

            _logger.LogInformation(
                "Face enrollment successful for user {UserId}. Status: {Status}, EmbeddingId: {EmbeddingId}",
                request.UserId, profile.Status, profile.EmbeddingId);

            return new FaceEnrollResponseDTO
            {
                Success = true,
                Status = profile.Status,
                RequiresPin = profile.Status == FaceAuthConstants.Status.PendingPin,
                NumImagesUsed = enrollResult.NumImagesUsed,
                ModelVersion = enrollResult.ModelVersion
            };
        }
    }
}
