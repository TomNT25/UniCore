using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class VerifyOtpRequestDTO : IRequest<VerifyOtpResponseDTO>
    {
        public string? Email { get; set; }
        public string? Username { get; set; }

        [JsonPropertyName("otpCode")]
        public string OtpCode { get; set; } = string.Empty;

        [JsonPropertyName("otp")]
        public string? Otp
        {
            get => OtpCode;
            set { if (!string.IsNullOrEmpty(value) && string.IsNullOrEmpty(OtpCode)) OtpCode = value; }
        }

        [JsonPropertyName("code")]
        public string? Code
        {
            get => OtpCode;
            set { if (!string.IsNullOrEmpty(value) && string.IsNullOrEmpty(OtpCode)) OtpCode = value; }
        }
    }
}
