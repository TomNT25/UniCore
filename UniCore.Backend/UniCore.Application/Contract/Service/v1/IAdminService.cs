using UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview;
using UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.DeleteUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.GetUserProfile;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IAdminService
    {
        Task<GetDashboardOverviewResponseDTO> GetDashboardOverviewAsync(GetDashboardOverviewRequestDTO request, CancellationToken cancellationToken = default);

        Task<GetAllUsersResponseDTO> GetAllUsersAsync(GetAllUsersRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetUserByIdResponseDTO> GetUserByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CreateUserResponseDTO> CreateUserAsync(CreateUserRequestDTO request, CancellationToken cancellationToken = default);
        Task<UpdateUserResponseDTO> UpdateUserAsync(UpdateUserRequestDTO request, CancellationToken cancellationToken = default);
        Task<DeleteUserResponseDTO> DeleteUserAsync(string id, CancellationToken cancellationToken = default);
        Task<UpdateUserStatusResponseDTO> UpdateUserStatusAsync(UpdateUserStatusRequestDTO request, CancellationToken cancellationToken = default);

        Task<GetUserProfileResponseDTO> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default);
        Task<UpdateUserProfileResponseDTO> UpdateUserProfileAsync(UpdateUserProfileRequestDTO request, CancellationToken cancellationToken = default);
    }
}
