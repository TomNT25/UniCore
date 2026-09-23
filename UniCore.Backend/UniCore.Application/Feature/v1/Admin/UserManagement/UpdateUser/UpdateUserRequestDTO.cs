using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser
{
    public class UpdateUserRequestDTO : IRequest<UpdateUserResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;
    }
}
