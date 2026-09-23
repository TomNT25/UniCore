
using UniCore.Application.Entity;
using Mapster;
namespace UniCore.Application.Feature.v1.User.GetUserInfo
{

    public class UserProfileMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserProfile, GetUserInfoResponseDTO>()
                .Map(dest => dest.FirstName, src => src.FirstName ?? string.Empty)
                .Map(dest => dest.LastName, src => src.LastName ?? string.Empty)
                .Map(dest => dest.FullName, src => src.FullName ?? string.Empty)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber ?? string.Empty)
                .Map(dest => dest.AvatarUrl, src => src.AvatarUrl ?? string.Empty)
                .Map(dest => dest.Gender, src => src.Gender ?? string.Empty)
                .Map(dest => dest.BirthDate, src => src.BirthDate.HasValue ? src.BirthDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.Address, src => src.Address ?? string.Empty)
                .Map(dest => dest.Bio, src => src.Bio ?? string.Empty);
        }
    }
}
