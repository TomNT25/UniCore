using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.User.GetUserPermission;

namespace UniCore.Application.Service.v1
{
    public class UserService : IUserService
    {
        private readonly GetUserPermissionHandler _getUserPermissionHandler;

        public UserService(GetUserPermissionHandler getUserPermissionHandler)
        {
            _getUserPermissionHandler = getUserPermissionHandler;
        }

        public Task<GetUserPermissionResponseDTO> GetUserPermissionAsync(GetUserPermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _getUserPermissionHandler.HandleAsync(request, cancellationToken);
    }
}
