using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.CreateAuditLog;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.DeleteAuditLog;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAuditLogById;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.UpdateAuditLog;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.CreateCourse;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.DeleteCourse;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.GetAllCourses;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.GetCourseById;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourse;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourseStatus;
using UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview;
using UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllStudents;
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

        Task<GetAllStudentsResponseDTO> GetAllStudentsAsync(GetAllStudentsRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetAllCoursesResponseDTO> GetAllCoursesAsync(GetAllCoursesRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetCourseByIdResponseDTO> GetCourseByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CreateCourseResponseDTO> CreateCourseAsync(CreateCourseRequestDTO request, CancellationToken cancellationToken = default);
        Task<UpdateCourseResponseDTO> UpdateCourseAsync(UpdateCourseRequestDTO request, CancellationToken cancellationToken = default);
        Task<DeleteCourseResponseDTO> DeleteCourseAsync(string id, CancellationToken cancellationToken = default);
        Task<UpdateCourseStatusResponseDTO> UpdateCourseStatusAsync(UpdateCourseStatusRequestDTO request, CancellationToken cancellationToken = default);

        Task<GetAllAuditLogsResponseDTO> GetAllAuditLogsAsync(GetAllAuditLogsRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetAuditLogByIdResponseDTO> GetAuditLogByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CreateAuditLogResponseDTO> CreateAuditLogAsync(CreateAuditLogRequestDTO request, CancellationToken cancellationToken = default);
        Task<UpdateAuditLogResponseDTO> UpdateAuditLogAsync(UpdateAuditLogRequestDTO request, CancellationToken cancellationToken = default);
        Task<DeleteAuditLogResponseDTO> DeleteAuditLogAsync(string id, CancellationToken cancellationToken = default);
    }
}
