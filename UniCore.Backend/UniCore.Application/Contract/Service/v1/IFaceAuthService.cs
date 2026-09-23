using UniCore.Application.Feature.v1.FaceAuth.Enroll;
using UniCore.Application.Feature.v1.FaceAuth.GetStatus;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Application.Feature.v1.FaceAuth.SetPin;
using UniCore.Application.Feature.v1.FaceAuth.VerifyPin;

namespace UniCore.Application.Contract.Service.v1
{
    /// <summary>
    /// Service for face authentication operations.
    /// </summary>
    public interface IFaceAuthService
    {
        /// <summary>
        /// Enroll face images for a user.
        /// </summary>
        Task<FaceEnrollResponseDTO> EnrollAsync(
            FaceEnrollRequestDTO request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Set PIN for face authentication.
        /// </summary>
        Task<SetPinResponseDTO> SetPinAsync(
            SetPinRequestDTO request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get face authentication status for a user.
        /// </summary>
        Task<GetFaceStatusResponseDTO> GetStatusAsync(
            GetFaceStatusRequestDTO request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Face login (recognize face and issue challenge token).
        /// </summary>
        Task<FaceLoginResponseDTO> LoginAsync(
            FaceLoginRequestDTO request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verify PIN and issue JWT tokens.
        /// </summary>
        Task<VerifyPinResponseDTO> VerifyPinAsync(
            VerifyPinRequestDTO request,
            CancellationToken cancellationToken = default);
    }
}
