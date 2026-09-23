using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.CreateRole
{
    public class CreateRoleRequestDTO : IRequest<CreateRoleResponseDTO>
    {
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
