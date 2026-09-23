using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs;

namespace UniCore.Application.MapperProfile
{
    public class AppLogMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AppLog, AppLogDTO>();
            config.NewConfig<AppLog, GetAllAuditLogsDTO>();
        }
    }
}
