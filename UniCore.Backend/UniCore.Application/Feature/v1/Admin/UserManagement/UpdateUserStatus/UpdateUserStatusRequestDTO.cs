using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus
{
    public class UpdateUserStatusRequestDTO : IRequest<UpdateUserStatusResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
