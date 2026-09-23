using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview;
using UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.DeleteUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers;
using UniCore.Application.Feature.v1.Admin.UserManagement.GetUserById;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser;
using UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.GetUserProfile;
using UniCore.Application.Feature.v1.Admin.UserProfileManagement.UpdateUserProfile;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/admin")]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _adminService = adminService;
        }

        // ==========================================
        // 1. DASHBOARD
        // ==========================================

        /// <summary>
        /// Get admin dashboard overview stats
        /// </summary>
        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetDashboardOverviewResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetDashboardOverviewResponseDTO>>> GetDashboardOverview([FromQuery] GetDashboardOverviewRequestDTO request)
        {
            var result = await _adminService.GetDashboardOverviewAsync(request);
            var message = _localizer.GetString(MessageConstants.Admin.GetDashboardSuccess);
            return OkResponse<GetDashboardOverviewResponseDTO>(result, message);
        }

        // ==========================================
        // 2. USER MANAGEMENT
        // ==========================================

        /// <summary>
        /// Get paged users list with filters
        /// </summary>
        [HttpGet("users")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllUsersResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllUsersResponseDTO>>> GetAllUsers([FromQuery] GetAllUsersRequestDTO request)
        {
            var result = await _adminService.GetAllUsersAsync(request);
            var message = _localizer.GetString(MessageConstants.Admin.GetAllUsersSuccess);
            return OkResponse<GetAllUsersResponseDTO>(result, message);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("users/{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetUserByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetUserByIdResponseDTO>>> GetUserById(string id)
        {
            var result = await _adminService.GetUserByIdAsync(id);
            if (result.User == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Admin.UserNotFound);
                return NotFoundResponse<GetUserByIdResponseDTO>(notFoundMessage);
            }

            var message = _localizer.GetString(MessageConstants.Admin.GetUserByIdSuccess);
            return OkResponse<GetUserByIdResponseDTO>(result, message);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost("users")]
        [ProducesResponseType(typeof(BaseAPIResponse<CreateUserResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreateUserResponseDTO>>> CreateUser([FromBody] CreateUserRequestDTO request)
        {
            var result = await _adminService.CreateUserAsync(request);
            var message = _localizer.GetString(MessageConstants.Admin.CreateUserSuccess);
            return CreatedResponse<CreateUserResponseDTO>(result, message);
        }

        /// <summary>
        /// Update user information
        /// </summary>
        [HttpPut("users/{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateUserResponseDTO>>> UpdateUser(string id, [FromBody] UpdateUserRequestDTO request)
        {
            request.Id = id;
            var result = await _adminService.UpdateUserAsync(request);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateUserSuccess);
            return OkResponse<UpdateUserResponseDTO>(result, message);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("users/{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeleteUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeleteUserResponseDTO>>> DeleteUser(string id)
        {
            var result = await _adminService.DeleteUserAsync(id);
            var message = _localizer.GetString(MessageConstants.Admin.DeleteUserSuccess);
            return OkResponse<DeleteUserResponseDTO>(result, message);
        }

        /// <summary>
        /// Update user active status
        /// </summary>
        [HttpPatch("users/{id}/status")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateUserStatusResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateUserStatusResponseDTO>>> UpdateUserStatus(string id, [FromBody] UpdateUserStatusRequestDTO request)
        {
            request.Id = id;
            var result = await _adminService.UpdateUserStatusAsync(request);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateUserStatusSuccess);
            return OkResponse<UpdateUserStatusResponseDTO>(result, message);
        }

        // ==========================================
        // 3. USER PROFILE MANAGEMENT
        // ==========================================

        /// <summary>
        /// Get profile for specific user
        /// </summary>
        [HttpGet("users/{userId}/profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetUserProfileResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetUserProfileResponseDTO>>> GetUserProfile(string userId)
        {
            var result = await _adminService.GetUserProfileAsync(userId);
            if (result.Profile == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Admin.ProfileNotFound);
                return NotFoundResponse<GetUserProfileResponseDTO>(notFoundMessage);
            }

            var message = _localizer.GetString(MessageConstants.Admin.GetUserProfileSuccess);
            return OkResponse<GetUserProfileResponseDTO>(result, message);
        }

        /// <summary>
        /// Update profile for specific user
        /// </summary>
        [HttpPut("users/{userId}/profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateUserProfileResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateUserProfileResponseDTO>>> UpdateUserProfile(string userId, [FromBody] UpdateUserProfileRequestDTO request)
        {
            request.UserId = userId;
            var result = await _adminService.UpdateUserProfileAsync(request);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateUserProfileSuccess);
            return OkResponse<UpdateUserProfileResponseDTO>(result, message);
        }
    }
}
