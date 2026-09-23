using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;

namespace UniCore.Application.MapperProfile
{
    public class UserProfileMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserProfile, UserProfileDTO>();
        }
    }
}
