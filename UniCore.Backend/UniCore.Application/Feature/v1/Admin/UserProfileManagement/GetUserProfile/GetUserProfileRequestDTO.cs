using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserProfileManagement.GetUserProfile
{
    public class GetUserProfileRequestDTO : IRequest<GetUserProfileResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
