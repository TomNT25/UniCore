
using UniCore.Application.Entity;
using Mapster;
namespace UniCore.Application.Feature.v1.User.PutUserInfo
{

    public class UserProfileMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PutUserInfoDTO, UserProfile>()
                .Map(dest => dest.Bio, src => src.Bio);
        }
    }
}
