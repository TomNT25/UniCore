using Mapster;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.User, GetClassFriendsDTO>()
                .Map(dest => dest.Name, src => src.Username ?? string.Empty)
                .Map(dest => dest.Email, src => src.Email ?? string.Empty);
        }
    }
}
