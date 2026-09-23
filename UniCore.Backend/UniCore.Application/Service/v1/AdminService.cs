using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview;
using UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.DeleteUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.GetUserProfile;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile;

namespace UniCore.Application.Service.v1
{
    public class AdminService : IAdminService
    {
        private readonly GetDashboardOverviewHandler _getDashboardOverviewHandler;
        private readonly GetAllUsersHandler _getAllUsersHandler;
        private readonly GetUserByIdHandler _getUserByIdHandler;
        private readonly CreateUserHandler _createUserHandler;
        private readonly UpdateUserHandler _updateUserHandler;
        private readonly DeleteUserHandler _deleteUserHandler;
        private readonly UpdateUserStatusHandler _updateUserStatusHandler;
        private readonly GetUserProfileHandler _getUserProfileHandler;
        private readonly UpdateUserProfileHandler _updateUserProfileHandler;

        public AdminService(
            GetDashboardOverviewHandler getDashboardOverviewHandler,
            GetAllUsersHandler getAllUsersHandler,
            GetUserByIdHandler getUserByIdHandler,
            CreateUserHandler createUserHandler,
            UpdateUserHandler updateUserHandler,
            DeleteUserHandler deleteUserHandler,
            UpdateUserStatusHandler updateUserStatusHandler,
            GetUserProfileHandler getUserProfileHandler,
            UpdateUserProfileHandler updateUserProfileHandler)
        {
            _getDashboardOverviewHandler = getDashboardOverviewHandler;
            _getAllUsersHandler = getAllUsersHandler;
            _getUserByIdHandler = getUserByIdHandler;
            _createUserHandler = createUserHandler;
            _updateUserHandler = updateUserHandler;
            _deleteUserHandler = deleteUserHandler;
            _updateUserStatusHandler = updateUserStatusHandler;
            _getUserProfileHandler = getUserProfileHandler;
            _updateUserProfileHandler = updateUserProfileHandler;
        }

        public Task<GetDashboardOverviewResponseDTO> GetDashboardOverviewAsync(GetDashboardOverviewRequestDTO request, CancellationToken cancellationToken = default)
            => _getDashboardOverviewHandler.HandleAsync(request, cancellationToken);

        public Task<GetAllUsersResponseDTO> GetAllUsersAsync(GetAllUsersRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllUsersHandler.HandleAsync(request, cancellationToken);

        public Task<GetUserByIdResponseDTO> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getUserByIdHandler.HandleAsync(new GetUserByIdRequestDTO { Id = id }, cancellationToken);

        public Task<CreateUserResponseDTO> CreateUserAsync(CreateUserRequestDTO request, CancellationToken cancellationToken = default)
            => _createUserHandler.HandleAsync(request, cancellationToken);

        public Task<UpdateUserResponseDTO> UpdateUserAsync(UpdateUserRequestDTO request, CancellationToken cancellationToken = default)
            => _updateUserHandler.HandleAsync(request, cancellationToken);

        public Task<DeleteUserResponseDTO> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
            => _deleteUserHandler.HandleAsync(new DeleteUserRequestDTO { Id = id }, cancellationToken);

        public Task<UpdateUserStatusResponseDTO> UpdateUserStatusAsync(UpdateUserStatusRequestDTO request, CancellationToken cancellationToken = default)
            => _updateUserStatusHandler.HandleAsync(request, cancellationToken);

        public Task<GetUserProfileResponseDTO> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default)
            => _getUserProfileHandler.HandleAsync(new GetUserProfileRequestDTO { UserId = userId }, cancellationToken);

        public Task<UpdateUserProfileResponseDTO> UpdateUserProfileAsync(UpdateUserProfileRequestDTO request, CancellationToken cancellationToken = default)
            => _updateUserProfileHandler.HandleAsync(request, cancellationToken);
    }
}
