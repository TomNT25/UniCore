using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers
{
    public class GetAllUsersRequestDTO : PageNumberPaginationRequest, IRequest<GetAllUsersResponseDTO>
    {
        public string? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEmailVerified { get; set; }
    }
}
