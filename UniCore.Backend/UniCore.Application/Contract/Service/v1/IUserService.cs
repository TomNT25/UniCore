using UniCore.Application.Feature.v1.User.GetUserPermission;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IUserService
    {
        Task<GetUserPermissionResponseDTO> GetUserPermissionAsync(GetUserPermissionRequestDTO request, CancellationToken cancellationToken = default);
    }
}
