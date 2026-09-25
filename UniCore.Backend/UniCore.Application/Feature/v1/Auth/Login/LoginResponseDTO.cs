using System.Text.Json.Serialization;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Auth.Login
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;

        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonIgnore]
        public int RefreshTokenExpire { get; set; }
        public UserLoginResponseDTO? User { get; set; }

        public bool RequiresMfa { get; set; } = false;
        public string? Email { get; set; }
        public string? MaskedEmail { get; set; }
        public string? Message { get; set; }
    }

    public class UserLoginResponseDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
    }
}