using Mapster;
using UniCore.Application.Entity;


namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class UsersClassMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SchoolClass, GetClassInfoResponseDTO>()
                .Map(dest => dest.Name, src => src.Name ?? string.Empty)
                .Map(dest => dest.Descriptions, src => src.Description ?? string.Empty)
                .Map(dest => dest.Code, src => src.Code ?? string.Empty)
                .Map(dest => dest.Id, src => src.Id ?? string.Empty)
                .Map(dest => dest.Department, src => src.Department != null ? new GetClassDepartmentDTO
                 {
                     Id = src.Department.Id,
                     Code = src.Department.Code,
                     Name = src.Department.Name,
                     Description = src.Department.Description
                 } : null);
        }
    }
}
