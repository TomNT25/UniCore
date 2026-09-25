using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.User.PutUserInfo
{
    public sealed class PutUserInfoRequestDTO : IRequest<PutUserInfoResponseDTO>
    {
        public string UserID { get; set; } = string.Empty;

        public PutUserInfoDTO PutUserBio { get; set; }
    }
}
