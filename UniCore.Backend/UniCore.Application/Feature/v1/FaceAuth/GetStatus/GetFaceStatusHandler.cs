using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.GetStatus
{
    public class GetFaceStatusHandler : IRequestHandler<GetFaceStatusRequestDTO, GetFaceStatusResponseDTO>
    {
        private readonly IUserFaceProfileRepository _faceProfileRepository;
        private readonly ILogger<GetFaceStatusHandler> _logger;

        public GetFaceStatusHandler(
            IUserFaceProfileRepository faceProfileRepository,
            ILogger<GetFaceStatusHandler> logger)
        {
            _faceProfileRepository = faceProfileRepository;
            _logger = logger;
        }

        public async Task<GetFaceStatusResponseDTO> HandleAsync(
            GetFaceStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Getting face status for user {UserId}", request.UserId);

            var profile = await _faceProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (profile == null)
            {
                _logger.LogDebug("No face profile found for user {UserId}", request.UserId);
                return new GetFaceStatusResponseDTO
                {
                    Status = FaceAuthConstants.Status.NotEnrolled,
                    IsEnrolled = false,
                    RequiresPin = false,
                    IsSuspended = false,
                    IsLocked = false
                };
            }

            var isLocked = profile.PinLockoutEnd.HasValue && profile.PinLockoutEnd > DateTime.UtcNow;

            return new GetFaceStatusResponseDTO
            {
                Status = profile.Status,
                IsEnrolled = profile.Status == FaceAuthConstants.Status.Enrolled,
                RequiresPin = profile.Status == FaceAuthConstants.Status.PendingPin,
                IsSuspended = profile.Status == FaceAuthConstants.Status.Suspended,
                IsLocked = isLocked,
                EnrolledAt = profile.EnrolledAt,
                PinSetAt = profile.PinSetAt,
                LockoutEnd = isLocked ? profile.PinLockoutEnd : null,
                ModelVersion = profile.ModelVersion
            };
        }
    }
}
