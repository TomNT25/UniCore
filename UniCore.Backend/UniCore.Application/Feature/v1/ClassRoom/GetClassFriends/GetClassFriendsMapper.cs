using Mapster;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SchoolClass, GetClassFriendsDTO>()
                .Map(dest => dest.Name, src => src.Students.FirstOrDefault()?. ?? string.Empty)
                .Map(dest => dest.PhoneNum, src => src.PhoneNumber ?? string.Empty)
                .Map(dest => dest.AvatarUrl, src => src.AvatarUrl ?? string.Empty);
        }
    }
}
