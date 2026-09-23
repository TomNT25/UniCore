using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.DeleteUser
{
    public class DeleteUserRequestDTO : IRequest<DeleteUserResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
