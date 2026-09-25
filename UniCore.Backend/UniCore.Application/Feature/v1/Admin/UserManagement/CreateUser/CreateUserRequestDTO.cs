using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserRequestDTO : IRequest<CreateUserResponseDTO>
    {
        [JsonPropertyName("student_code")]
        public string? StudentCode { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RoleId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public string? AdminUserId { get; set; }
    }
}
