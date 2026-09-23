using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.UpdatePermission
{
    public class UpdatePermissionRequestDTO : IRequest<UpdatePermissionResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
