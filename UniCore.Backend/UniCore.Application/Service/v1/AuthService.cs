using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Auth.ChangePassword;
using UniCore.Application.Feature.v1.Auth.ForgotPassword;
using UniCore.Application.Feature.v1.Auth.GoogleAuth.GoogleLogin;
using UniCore.Application.Feature.v1.Auth.GoogleAuth.SimulateGoogleToken;
using UniCore.Application.Feature.v1.Auth.Login;
using UniCore.Application.Feature.v1.Auth.Logout;
using UniCore.Application.Feature.v1.Auth.Me;
using UniCore.Application.Feature.v1.Auth.Mfa.DisableMfa;
using UniCore.Application.Feature.v1.Auth.Mfa.EnableMfa;
using UniCore.Application.Feature.v1.Auth.Mfa.SetupMfa;
using UniCore.Application.Feature.v1.Auth.Mfa.VerifyMfa;
using UniCore.Application.Feature.v1.Auth.RefreshToken;
using UniCore.Application.Feature.v1.Auth.Register;
using UniCore.Application.Feature.v1.Auth.VerifyOtp;

namespace UniCore.Application.Service.v1
{
    public class AuthService : IAuthService
    {
        private readonly LoginHandler _loginHandler;
        private readonly RefreshTokenHandler _refreshTokenHandler;
        private readonly RegisterHandler _registerHandler;
        private readonly LogoutHandler _logoutHandler;
        private readonly SendOtpHandler _sendOtpHandler;
        private readonly VerifyOtpHandler _verifyOtpHandler;
        private readonly ForgotPasswordHandler _forgotPasswordHandler;
        private readonly ResetPasswordHandler _resetPasswordHandler;
        private readonly GetMeQueryHandler _getMeQueryHandler;
        private readonly ChangePasswordHandler _changePasswordHandler;
        private readonly SetupMfaHandler _setupMfaHandler;
        private readonly EnableMfaHandler _enableMfaHandler;
        private readonly DisableMfaHandler _disableMfaHandler;
        private readonly VerifyMfaHandler _verifyMfaHandler;
        private readonly SimulateGoogleTokenHandler _simulateGoogleTokenHandler;
        private readonly GoogleLoginHandler _googleLoginHandler;

        public AuthService(
            LoginHandler loginHandler,
            RefreshTokenHandler refreshTokenHandler,
            RegisterHandler registerHandler,
            LogoutHandler logoutHandler,
            SendOtpHandler sendOtpHandler,
            VerifyOtpHandler verifyOtpHandler,
            ForgotPasswordHandler forgotPasswordHandler,
            ResetPasswordHandler resetPasswordHandler,
            GetMeQueryHandler getMeQueryHandler,
            ChangePasswordHandler changePasswordHandler,
            SetupMfaHandler setupMfaHandler,
            EnableMfaHandler enableMfaHandler,
            DisableMfaHandler disableMfaHandler,
            VerifyMfaHandler verifyMfaHandler,
            SimulateGoogleTokenHandler simulateGoogleTokenHandler,
            GoogleLoginHandler googleLoginHandler)
        {
            _loginHandler = loginHandler;
            _refreshTokenHandler = refreshTokenHandler;
            _registerHandler = registerHandler;
            _logoutHandler = logoutHandler;
            _sendOtpHandler = sendOtpHandler;
            _verifyOtpHandler = verifyOtpHandler;
            _forgotPasswordHandler = forgotPasswordHandler;
            _resetPasswordHandler = resetPasswordHandler;
            _getMeQueryHandler = getMeQueryHandler;
            _changePasswordHandler = changePasswordHandler;
            _setupMfaHandler = setupMfaHandler;
            _enableMfaHandler = enableMfaHandler;
            _disableMfaHandler = disableMfaHandler;
            _verifyMfaHandler = verifyMfaHandler;
            _simulateGoogleTokenHandler = simulateGoogleTokenHandler;
            _googleLoginHandler = googleLoginHandler;
        }

        public Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken = default)
            => _loginHandler.HandleAsync(request, cancellationToken);

        public Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request, CancellationToken cancellationToken = default)
            => _refreshTokenHandler.HandleAsync(request, cancellationToken);

        public Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken = default)
            => _registerHandler.HandleAsync(request, cancellationToken);

        public Task<LogoutResponseDTO> LogoutAsync(LogoutRequestDTO request, CancellationToken cancellationToken = default)
            => _logoutHandler.HandleAsync(request, cancellationToken);

        public Task<SendOtpResponseDTO> SendOtpAsync(SendOtpRequestDTO request, CancellationToken cancellationToken = default)
            => _sendOtpHandler.HandleAsync(request, cancellationToken);

        public Task<VerifyOtpResponseDTO> VerifyOtpAsync(VerifyOtpRequestDTO request, CancellationToken cancellationToken = default)
            => _verifyOtpHandler.HandleAsync(request, cancellationToken);

        public Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request, CancellationToken cancellationToken = default)
            => _forgotPasswordHandler.HandleAsync(request, cancellationToken);

        public Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request, CancellationToken cancellationToken = default)
            => _resetPasswordHandler.HandleAsync(request, cancellationToken);

        public Task<GetMeResponseDTO?> GetMeAsync(string userId, CancellationToken cancellationToken = default)
            => _getMeQueryHandler.HandleAsync(new GetMeQuery(userId), cancellationToken);

        public Task<ChangePasswordResponseDTO> ChangePasswordAsync(ChangePasswordRequestDTO request, CancellationToken cancellationToken = default)
            => _changePasswordHandler.HandleAsync(request, cancellationToken);

        public Task<SetupMfaResponseDTO> SetupMfaAsync(SetupMfaRequestDTO request, CancellationToken cancellationToken = default)
            => _setupMfaHandler.HandleAsync(request, cancellationToken);

        public Task<EnableMfaResponseDTO> EnableMfaAsync(EnableMfaRequestDTO request, CancellationToken cancellationToken = default)
            => _enableMfaHandler.HandleAsync(request, cancellationToken);

        public Task<DisableMfaResponseDTO> DisableMfaAsync(DisableMfaRequestDTO request, CancellationToken cancellationToken = default)
            => _disableMfaHandler.HandleAsync(request, cancellationToken);

        public Task<VerifyMfaResponseDTO> VerifyMfaAsync(VerifyMfaRequestDTO request, CancellationToken cancellationToken = default)
            => _verifyMfaHandler.HandleAsync(request, cancellationToken);

        public Task<SimulateGoogleTokenResponseDTO> SimulateGoogleTokenAsync(SimulateGoogleTokenRequestDTO request, CancellationToken cancellationToken = default)
            => _simulateGoogleTokenHandler.HandleAsync(request, cancellationToken);

        public Task<LoginResponseDTO> GoogleLoginAsync(GoogleLoginRequestDTO request, CancellationToken cancellationToken = default)
            => _googleLoginHandler.HandleAsync(request, cancellationToken);
    }
}
