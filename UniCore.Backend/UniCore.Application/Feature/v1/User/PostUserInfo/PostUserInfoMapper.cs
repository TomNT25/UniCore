
using UniCore.Application.Entity;
using Mapster;
using System.Globalization;
namespace UniCore.Application.Feature.v1.User.PostUserInfo
{

    public class UserProfileMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PostUserInfoRequestDTO, UserProfile>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.FullName, src => src.FullName)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.Gender, src => src.Gender)
                .Map(dest => dest.BirthDate, src => DateTime.Parse(src.BirthDate, CultureInfo.InvariantCulture))
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.Bio, src => src.Bio);
        }
    }
}
