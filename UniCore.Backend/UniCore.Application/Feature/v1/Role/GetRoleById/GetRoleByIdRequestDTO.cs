using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.GetRoleById
{
    public class GetRoleByIdRequestDTO : IRequest<GetRoleByIdResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
