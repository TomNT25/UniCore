using System.Text.Json.Serialization;
using UniCore.Application.Feature.v1.Auth.Login;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class VerifyOtpResponseDTO
    {
        public string Email { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = true;
        public string Message { get; set; } = "Email verified successfully";

        public string? AccessToken { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        [JsonIgnore]
        public int RefreshTokenExpire { get; set; }

        public UserLoginResponseDTO? User { get; set; }
    }
}
