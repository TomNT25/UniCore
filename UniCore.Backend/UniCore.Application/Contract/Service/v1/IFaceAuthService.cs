using UniCore.Application.Feature.v1.FaceAuth.Enroll;
using UniCore.Application.Feature.v1.FaceAuth.GetStatus;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Application.Feature.v1.FaceAuth.Mfa.DisableFaceMfa;
using UniCore.Application.Feature.v1.FaceAuth.Mfa.EnableFaceMfa;
using UniCore.Application.Feature.v1.FaceAuth.ResendOtp;
using UniCore.Application.Feature.v1.FaceAuth.SetPin;
using UniCore.Application.Feature.v1.FaceAuth.VerifyOtp;
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

        /// <summary>
        /// Enable Face Recognition MFA.
        /// </summary>
        Task<EnableFaceMfaResponseDTO> EnableMfaAsync(
            EnableFaceMfaRequestDTO request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Disable Face Recognition MFA.
        /// </summary>
        Task<DisableFaceMfaResponseDTO> DisableMfaAsync(
            DisableFaceMfaRequestDTO request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verify OTP for face login and issue JWT tokens.
        /// </summary>
        Task<VerifyOtpResponseDTO> VerifyOtpAsync(
            VerifyOtpRequestDTO request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Resend OTP for face login challenge.
        /// </summary>
        Task<ResendOtpResponseDTO> ResendOtpAsync(
            ResendOtpRequestDTO request,
            CancellationToken cancellationToken = default);
    }
}
