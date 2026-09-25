using UniCore.Application.Contract.Service.v1;
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
using UniCore.Application.Feature.v1.Admin.UserManagement.CreateBulkStudentAccounts;
using UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.DeleteUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.GetUserProfile;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.VerifyUserProfile;

namespace UniCore.Application.Service.v1
{
    public class AdminService : IAdminService
    {
        private readonly GetDashboardOverviewHandler _getDashboardOverviewHandler;
        private readonly GetAllUsersHandler _getAllUsersHandler;
        private readonly GetUserByIdHandler _getUserByIdHandler;
        private readonly CreateUserHandler _createUserHandler;
        private readonly CreateBulkStudentAccountsHandler _createBulkStudentAccountsHandler;
        private readonly UpdateUserHandler _updateUserHandler;
        private readonly DeleteUserHandler _deleteUserHandler;
        private readonly UpdateUserStatusHandler _updateUserStatusHandler;
        private readonly GetUserProfileHandler _getUserProfileHandler;
        private readonly UpdateUserProfileHandler _updateUserProfileHandler;
        private readonly VerifyUserProfileHandler _verifyUserProfileHandler;
        private readonly GetAllStudentsHandler _getAllStudentsHandler;
        private readonly GetAllCoursesHandler _getAllCoursesHandler;
        private readonly GetCourseByIdHandler _getCourseByIdHandler;
        private readonly CreateCourseHandler _createCourseHandler;
        private readonly UpdateCourseHandler _updateCourseHandler;
        private readonly DeleteCourseHandler _deleteCourseHandler;
        private readonly UpdateCourseStatusHandler _updateCourseStatusHandler;

        private readonly GetAllAuditLogsHandler _getAllAuditLogsHandler;
        private readonly GetAuditLogByIdHandler _getAuditLogByIdHandler;
        private readonly CreateAuditLogHandler _createAuditLogHandler;
        private readonly UpdateAuditLogHandler _updateAuditLogHandler;
        private readonly DeleteAuditLogHandler _deleteAuditLogHandler;

        public AdminService(
            GetDashboardOverviewHandler getDashboardOverviewHandler,
            GetAllUsersHandler getAllUsersHandler,
            GetUserByIdHandler getUserByIdHandler,
            CreateUserHandler createUserHandler,
            CreateBulkStudentAccountsHandler createBulkStudentAccountsHandler,
            UpdateUserHandler updateUserHandler,
            DeleteUserHandler deleteUserHandler,
            UpdateUserStatusHandler updateUserStatusHandler,
            GetUserProfileHandler getUserProfileHandler,
            UpdateUserProfileHandler updateUserProfileHandler,
            VerifyUserProfileHandler verifyUserProfileHandler,
            GetAllStudentsHandler getAllStudentsHandler,
            GetAllCoursesHandler getAllCoursesHandler,
            GetCourseByIdHandler getCourseByIdHandler,
            CreateCourseHandler createCourseHandler,
            UpdateCourseHandler updateCourseHandler,
            DeleteCourseHandler deleteCourseHandler,
            UpdateCourseStatusHandler updateCourseStatusHandler,
            GetAllAuditLogsHandler getAllAuditLogsHandler,
            GetAuditLogByIdHandler getAuditLogByIdHandler,
            CreateAuditLogHandler createAuditLogHandler,
            UpdateAuditLogHandler updateAuditLogHandler,
            DeleteAuditLogHandler deleteAuditLogHandler)
        {
            _getDashboardOverviewHandler = getDashboardOverviewHandler;
            _getAllUsersHandler = getAllUsersHandler;
            _getUserByIdHandler = getUserByIdHandler;
            _createUserHandler = createUserHandler;
            _createBulkStudentAccountsHandler = createBulkStudentAccountsHandler;
            _updateUserHandler = updateUserHandler;
            _deleteUserHandler = deleteUserHandler;
            _updateUserStatusHandler = updateUserStatusHandler;
            _getUserProfileHandler = getUserProfileHandler;
            _updateUserProfileHandler = updateUserProfileHandler;
            _verifyUserProfileHandler = verifyUserProfileHandler;
            _getAllStudentsHandler = getAllStudentsHandler;
            _getAllCoursesHandler = getAllCoursesHandler;
            _getCourseByIdHandler = getCourseByIdHandler;
            _createCourseHandler = createCourseHandler;
            _updateCourseHandler = updateCourseHandler;
            _deleteCourseHandler = deleteCourseHandler;
            _updateCourseStatusHandler = updateCourseStatusHandler;
            _getAllAuditLogsHandler = getAllAuditLogsHandler;
            _getAuditLogByIdHandler = getAuditLogByIdHandler;
            _createAuditLogHandler = createAuditLogHandler;
            _updateAuditLogHandler = updateAuditLogHandler;
            _deleteAuditLogHandler = deleteAuditLogHandler;
        }

        public Task<GetDashboardOverviewResponseDTO> GetDashboardOverviewAsync(GetDashboardOverviewRequestDTO request, CancellationToken cancellationToken = default)
            => _getDashboardOverviewHandler.HandleAsync(request, cancellationToken);

        public Task<GetAllUsersResponseDTO> GetAllUsersAsync(GetAllUsersRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllUsersHandler.HandleAsync(request, cancellationToken);

        public Task<GetUserByIdResponseDTO> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getUserByIdHandler.HandleAsync(new GetUserByIdRequestDTO { Id = id }, cancellationToken);

        public Task<CreateUserResponseDTO> CreateUserAsync(CreateUserRequestDTO request, CancellationToken cancellationToken = default)
            => _createUserHandler.HandleAsync(request, cancellationToken);

        public Task<CreateBulkStudentAccountsResponseDTO> CreateBulkStudentAccountsAsync(CreateBulkStudentAccountsRequestDTO request, CancellationToken cancellationToken = default)
            => _createBulkStudentAccountsHandler.HandleAsync(request, cancellationToken);

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

        public Task<VerifyUserProfileResponseDTO> VerifyUserProfileAsync(VerifyUserProfileRequestDTO request, CancellationToken cancellationToken = default)
            => _verifyUserProfileHandler.HandleAsync(request, cancellationToken);

        public Task<GetAllStudentsResponseDTO> GetAllStudentsAsync(GetAllStudentsRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllStudentsHandler.HandleAsync(request, cancellationToken);

        public Task<GetAllCoursesResponseDTO> GetAllCoursesAsync(GetAllCoursesRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllCoursesHandler.HandleAsync(request, cancellationToken);

        public Task<GetCourseByIdResponseDTO> GetCourseByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getCourseByIdHandler.HandleAsync(new GetCourseByIdRequestDTO { Id = id }, cancellationToken);

        public Task<CreateCourseResponseDTO> CreateCourseAsync(CreateCourseRequestDTO request, CancellationToken cancellationToken = default)
            => _createCourseHandler.HandleAsync(request, cancellationToken);

        public Task<UpdateCourseResponseDTO> UpdateCourseAsync(UpdateCourseRequestDTO request, CancellationToken cancellationToken = default)
            => _updateCourseHandler.HandleAsync(request, cancellationToken);

        public Task<DeleteCourseResponseDTO> DeleteCourseAsync(string id, CancellationToken cancellationToken = default)
            => _deleteCourseHandler.HandleAsync(new DeleteCourseRequestDTO { Id = id }, cancellationToken);

        public Task<UpdateCourseStatusResponseDTO> UpdateCourseStatusAsync(UpdateCourseStatusRequestDTO request, CancellationToken cancellationToken = default)
            => _updateCourseStatusHandler.HandleAsync(request, cancellationToken);

        public Task<GetAllAuditLogsResponseDTO> GetAllAuditLogsAsync(GetAllAuditLogsRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllAuditLogsHandler.HandleAsync(request, cancellationToken);

        public Task<GetAuditLogByIdResponseDTO> GetAuditLogByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getAuditLogByIdHandler.HandleAsync(new GetAuditLogByIdRequestDTO { Id = id }, cancellationToken);

        public Task<CreateAuditLogResponseDTO> CreateAuditLogAsync(CreateAuditLogRequestDTO request, CancellationToken cancellationToken = default)
            => _createAuditLogHandler.HandleAsync(request, cancellationToken);

        public Task<UpdateAuditLogResponseDTO> UpdateAuditLogAsync(UpdateAuditLogRequestDTO request, CancellationToken cancellationToken = default)
            => _updateAuditLogHandler.HandleAsync(request, cancellationToken);

        public Task<DeleteAuditLogResponseDTO> DeleteAuditLogAsync(string id, CancellationToken cancellationToken = default)
            => _deleteAuditLogHandler.HandleAsync(new DeleteAuditLogRequestDTO { Id = id }, cancellationToken);
    }
}
