using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.User.GetUserInfo
{
    public sealed class GetUserInfoRequestDTO : IRequest<GetUserInfoResponseDTO>
    {
        public string UserID { get; set; } = string.Empty;
    }
}
