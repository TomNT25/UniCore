using Mapster;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.ClassRoom.GetClassFriends
{
    public class GetClassFriendsMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserProfile, GetClassFriendsDTO>()
                .Map(dest => dest.FriendName, src => src.FullName ?? string.Empty)
                .Map(dest => dest.FriendPhoneNum, src => src.PhoneNumber ?? string.Empty)
                .Map(dest => dest.FriendAvatarUrl, src => src.AvatarUrl ?? string.Empty);
        }
    }
}
