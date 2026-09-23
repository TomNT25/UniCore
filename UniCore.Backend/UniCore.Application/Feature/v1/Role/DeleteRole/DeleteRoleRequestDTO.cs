using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.DeleteRole
{
    public class DeleteRoleRequestDTO : IRequest<DeleteRoleResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
