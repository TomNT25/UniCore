namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.VerifyUserProfile
{
    public class VerifyUserProfileResponseDTO
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? VerifiedBy { get; set; }
        public UserProfileDetailDTO? Profile { get; set; }
    }
}
