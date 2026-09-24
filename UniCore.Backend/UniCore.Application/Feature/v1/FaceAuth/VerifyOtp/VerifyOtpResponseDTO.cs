using UniCore.Application.Feature.v1.Auth.Login;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyOtp
{
    public class VerifyOtpResponseDTO
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int? RefreshTokenExpire { get; set; }
        public UserLoginResponseDTO? User { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public int? RemainingAttempts { get; set; }
    }
}
