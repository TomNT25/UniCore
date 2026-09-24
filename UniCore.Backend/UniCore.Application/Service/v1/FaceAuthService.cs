using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.FaceAuth.Enroll;
using UniCore.Application.Feature.v1.FaceAuth.GetStatus;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Application.Feature.v1.FaceAuth.Mfa.DisableFaceMfa;
using UniCore.Application.Feature.v1.FaceAuth.Mfa.EnableFaceMfa;
using UniCore.Application.Feature.v1.FaceAuth.ResendOtp;
using UniCore.Application.Feature.v1.FaceAuth.SetPin;
using UniCore.Application.Feature.v1.FaceAuth.VerifyOtp;
using UniCore.Application.Feature.v1.FaceAuth.VerifyPin;

namespace UniCore.Application.Service.v1
{
    public class FaceAuthService : IFaceAuthService
    {
        private readonly FaceEnrollHandler _enrollHandler;
        private readonly SetPinHandler _setPinHandler;
        private readonly GetFaceStatusHandler _getStatusHandler;
        private readonly FaceLoginHandler _loginHandler;
        private readonly VerifyPinHandler _verifyPinHandler;
        private readonly EnableFaceMfaHandler _enableMfaHandler;
        private readonly DisableFaceMfaHandler _disableMfaHandler;
        private readonly VerifyOtpHandler _verifyOtpHandler;
        private readonly ResendOtpHandler _resendOtpHandler;

        public FaceAuthService(
            FaceEnrollHandler enrollHandler,
            SetPinHandler setPinHandler,
            GetFaceStatusHandler getStatusHandler,
            FaceLoginHandler loginHandler,
            VerifyPinHandler verifyPinHandler,
            EnableFaceMfaHandler enableMfaHandler,
            DisableFaceMfaHandler disableMfaHandler,
            VerifyOtpHandler verifyOtpHandler,
            ResendOtpHandler resendOtpHandler)
        {
            _enrollHandler = enrollHandler;
            _setPinHandler = setPinHandler;
            _getStatusHandler = getStatusHandler;
            _loginHandler = loginHandler;
            _verifyPinHandler = verifyPinHandler;
            _enableMfaHandler = enableMfaHandler;
            _disableMfaHandler = disableMfaHandler;
            _verifyOtpHandler = verifyOtpHandler;
            _resendOtpHandler = resendOtpHandler;
        }

        public Task<FaceEnrollResponseDTO> EnrollAsync(
            FaceEnrollRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _enrollHandler.HandleAsync(request, cancellationToken);
        }

        public Task<SetPinResponseDTO> SetPinAsync(
            SetPinRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _setPinHandler.HandleAsync(request, cancellationToken);
        }

        public Task<GetFaceStatusResponseDTO> GetStatusAsync(
            GetFaceStatusRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _getStatusHandler.HandleAsync(request, cancellationToken);
        }

        public Task<FaceLoginResponseDTO> LoginAsync(
            FaceLoginRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _loginHandler.HandleAsync(request, cancellationToken);
        }

        public Task<VerifyPinResponseDTO> VerifyPinAsync(
            VerifyPinRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _verifyPinHandler.HandleAsync(request, cancellationToken);
        }

        public Task<EnableFaceMfaResponseDTO> EnableMfaAsync(
            EnableFaceMfaRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _enableMfaHandler.HandleAsync(request, cancellationToken);
        }

        public Task<DisableFaceMfaResponseDTO> DisableMfaAsync(
            DisableFaceMfaRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _disableMfaHandler.HandleAsync(request, cancellationToken);
        }

        public Task<VerifyOtpResponseDTO> VerifyOtpAsync(
            VerifyOtpRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _verifyOtpHandler.HandleAsync(request, cancellationToken);
        }

        public Task<ResendOtpResponseDTO> ResendOtpAsync(
            ResendOtpRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            return _resendOtpHandler.HandleAsync(request, cancellationToken);
        }
    }
}
