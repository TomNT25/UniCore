using Mapster;
using UniCore.Application.Feature.v1.Auth.Me;
using UniCore.Application.Feature.v1.Auth.Register;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Auth.Login;

namespace UniCore.Application.MapperProfile
{
    public class LoginMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserDTO, UserLoginResponseDTO>();
        }
    }
}