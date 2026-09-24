using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement;

namespace UniCore.Application.MapperProfile
{
    public class UserProfileMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserProfile, UserProfileDTO>();
            config.NewConfig<UserProfile, UserProfileDetailDTO>();
        }
    }
}
