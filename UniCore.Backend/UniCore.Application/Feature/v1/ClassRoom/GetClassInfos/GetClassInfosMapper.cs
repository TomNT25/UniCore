using Mapster;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class UsersClassMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SchoolClass, GetClassInfoResponseDTO>()
                .Map(dest => dest.ClassName, src => src.Name ?? string.Empty)
                .Map(dest => dest.ClassDescriptions, src => src.Description ?? string.Empty)
                .Map(dest => dest.ClassLecturers, src => src.Description ?? string.Empty);
        }
    }
}
