using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;

namespace UniCore.Application.MapperProfile
{
    public class PermissionMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Permission, PermissionDTO>();
            config.NewConfig<List<Permission>, List<PermissionDTO>>();
        }
    }
}