using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile
{
    public class UpdateUserProfileRequestDTO : IRequest<UpdateUserProfileResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public int? AvatarMediaFileId { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
    }
}
