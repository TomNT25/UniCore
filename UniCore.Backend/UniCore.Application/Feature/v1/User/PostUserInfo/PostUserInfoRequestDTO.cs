using UniCore.Application.Contract.RequestHandlerHub;
using System.Text.Json.Serialization;

namespace UniCore.Application.Feature.v1.User.PostUserInfo
{
    public sealed class PostUserInfoRequestDTO : IRequest<PostUserInfoResponseDTO>
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
    }
}
