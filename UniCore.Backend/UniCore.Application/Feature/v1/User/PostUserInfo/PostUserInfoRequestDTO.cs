using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.User.PostUserInfo
{
    public sealed class PostUserInfoRequestDTO : IRequest<PostUserInfoResponseDTO>
    {
        public string UserID { get; set; } = string.Empty;

        public PostUserInfoDTO PostUserBio { get; set; }
    }
}
