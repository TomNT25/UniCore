using Mapster;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAuditLogById;

namespace UniCore.Application.MapperProfile
{
    public class AppLogMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AppLog, AppLogDTO>();

            config.NewConfig<AppLog, GetAllAuditLogsDTO>()
                .Map(dest => dest.HasException, src => !string.IsNullOrWhiteSpace(src.Exception));

            config.NewConfig<AppLog, AuditLogDetailDTO>();

            config.NewConfig<AppLog, AuditLogSummaryDTO>();
        }
    }
}
