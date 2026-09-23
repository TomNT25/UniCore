using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Auth.Login
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int RefreshTokenExpire { get; set; }
        public UserLoginResponseDTO User { get; set; } = default!;
    }

    public class UserLoginResponseDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
        public List<PermissionDTO> Permissions { get; set; } = new List<PermissionDTO>();
    }
}