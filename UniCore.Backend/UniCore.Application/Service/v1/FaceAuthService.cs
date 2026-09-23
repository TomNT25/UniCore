using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.FaceAuth.Enroll;
using UniCore.Application.Feature.v1.FaceAuth.GetStatus;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Application.Feature.v1.FaceAuth.SetPin;
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

        public FaceAuthService(
            FaceEnrollHandler enrollHandler,
            SetPinHandler setPinHandler,
            GetFaceStatusHandler getStatusHandler,
            FaceLoginHandler loginHandler,
            VerifyPinHandler verifyPinHandler)
        {
            _enrollHandler = enrollHandler;
            _setPinHandler = setPinHandler;
            _getStatusHandler = getStatusHandler;
            _loginHandler = loginHandler;
            _verifyPinHandler = verifyPinHandler;
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
    }
}
