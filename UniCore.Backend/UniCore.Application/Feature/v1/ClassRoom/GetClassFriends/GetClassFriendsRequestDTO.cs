using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsRequestDTO : IRequest<GetClassFriendsResponseDTO>
    {
        public string UserID { get; set; }
        public string ClassID { get; set; }
    }
}
