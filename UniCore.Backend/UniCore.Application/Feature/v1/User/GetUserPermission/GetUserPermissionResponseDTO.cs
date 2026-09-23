using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.User.GetUserPermission
{
    public class GetUserPermissionResponseDTO
    {
        public List<PermissionDTO> Permissions { get; set; }
    }
}
