using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.VerifyUserProfile
{
    public class VerifyUserProfileRequestDTO : IRequest<VerifyUserProfileResponseDTO>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;

        public bool IsVerified { get; set; } = true;

        [JsonIgnore]
        public string? AdminUserId { get; set; }
    }
}
