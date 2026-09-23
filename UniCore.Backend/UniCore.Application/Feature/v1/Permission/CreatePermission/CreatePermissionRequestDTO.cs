using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.CreatePermission
{
    public class CreatePermissionRequestDTO : IRequest<CreatePermissionResponseDTO>
    {
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
