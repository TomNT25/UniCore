
using UniCore.Application.Entity;
using Mapster;
using System.Globalization;
namespace UniCore.Application.Feature.v1.User.PostUserInfo
{

    public class UserProfileMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PostUserInfoDTO, UserProfile>()
                .Map(dest => dest.FirstName, src => src.firstName)
                .Map(dest => dest.LastName, src => src.lastName)
                .Map(dest => dest.FullName, src => src.fullName)
                .Map(dest => dest.PhoneNumber, src => src.phoneNumber)
                .Map(dest => dest.Gender, src => src.gender)
                .Map(dest => dest.BirthDate, src => DateTime.Parse(src.birthDate, CultureInfo.InvariantCulture))
                .Map(dest => dest.Address, src => src.address)
                .Map(dest => dest.Bio, src => src.bio);
        }
    }
}
