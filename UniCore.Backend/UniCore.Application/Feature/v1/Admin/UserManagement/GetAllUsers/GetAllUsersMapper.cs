using Mapster;
using UserEntity = UniCore.Application.Entity.User;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers
{
    public class GetAllUsersMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserEntity, GetAllUsersItemDTO>()
                .Map(dest => dest.StudentCode, src => src.StudentCode ?? string.Empty)
                .Map(
                dest => dest.IsProfileVerified,
                src => src.UserProfile == null
                    ? default(bool?)
                    : (bool?)src.UserProfile.IsActive);
        }
    }
}