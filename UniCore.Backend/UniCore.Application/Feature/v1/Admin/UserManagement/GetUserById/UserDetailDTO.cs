using System.Text.Json.Serialization;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById
{
    public class UserDetailDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserDetailProfileDTO? Profile { get; set; }
        public List<string> Roles { get; set; } = new();
        public string Status { get; set; } = "active";
        public string Provider { get; set; } = "SYSTEM";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UserDetailProfileDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }

        [JsonPropertyName("cid_front_image_url")]
        public string? CidFrontImageUrl { get; set; }

        [JsonPropertyName("cid_back_image_url")]
        public string? CidBackImageUrl { get; set; }

        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = true;

        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }

        [JsonPropertyName("verified_at")]
        public DateTime? VerifiedAt { get; set; }
    }
}
