using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllStudents;
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

namespace UniCore.API.Controllers.v1.Admin
{
    [ApiVersion("1.0")]
    [Authorize(Roles = $"{DatabaseConstants.Roles.AdminName},{DatabaseConstants.Roles.AdminCode}")]
    [Route("api/v{version:apiVersion}/admin/users")]
    public class UserController : BaseController
    {
        private readonly IAdminService _adminService;

        public UserController(IAdminService adminService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _adminService = adminService;
        }

        // ==========================================
        // 1. USER MANAGEMENT
        // ==========================================

        /// <summary>
        /// Get paged users list with filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllUsersResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllUsersResponseDTO>>> GetAllUsers([FromQuery] GetAllUsersRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetAllUsersAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.GetAllUsersSuccess);
            return OkResponse<GetAllUsersResponseDTO>(result, message);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetUserByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetUserByIdResponseDTO>>> GetUserById(string id, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetUserByIdAsync(id, cancellationToken);
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
        [HttpPost]
        [ProducesResponseType(typeof(BaseAPIResponse<CreateUserResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreateUserResponseDTO>>> CreateUser([FromBody] CreateUserRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.CreateUserAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.CreateUserSuccess);
            return CreatedResponse<CreateUserResponseDTO>(result, message);
        }

        /// <summary>
        /// Update user information
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateUserResponseDTO>>> UpdateUser(string id, [FromBody] UpdateUserRequestDTO request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _adminService.UpdateUserAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateUserSuccess);
            return OkResponse<UpdateUserResponseDTO>(result, message);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeleteUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeleteUserResponseDTO>>> DeleteUser(string id, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.DeleteUserAsync(id, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.DeleteUserSuccess);
            return OkResponse<DeleteUserResponseDTO>(result, message);
        }

        /// <summary>
        /// Update user active status
        /// </summary>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateUserStatusResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateUserStatusResponseDTO>>> UpdateUserStatus(string id, [FromBody] UpdateUserStatusRequestDTO request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _adminService.UpdateUserStatusAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateUserStatusSuccess);
            return OkResponse<UpdateUserStatusResponseDTO>(result, message);
        }

        // ==========================================
        // 2. USER PROFILE MANAGEMENT
        // ==========================================

        /// <summary>
        /// Get profile for specific user
        /// </summary>
        [HttpGet("{userId}/profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetUserProfileResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetUserProfileResponseDTO>>> GetUserProfile(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetUserProfileAsync(userId, cancellationToken);
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
        [HttpPut("{userId}/profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateUserProfileResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateUserProfileResponseDTO>>> UpdateUserProfile(string userId, [FromBody] UpdateUserProfileRequestDTO request, CancellationToken cancellationToken = default)
        {
            request.UserId = userId;
            var result = await _adminService.UpdateUserProfileAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateUserProfileSuccess);
            return OkResponse<UpdateUserProfileResponseDTO>(result, message);
        }

        // ==========================================
        // 3. STUDENT (USER) MANAGEMENT
        // ==========================================

        /// <summary>
        /// Get paged students list
        /// </summary>
        [HttpGet("students")]
        [HttpGet("~/api/v{version:apiVersion}/admin/students")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllStudentsResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllStudentsResponseDTO>>> GetAllStudents([FromQuery] GetAllStudentsRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetAllStudentsAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.GetAllStudentsSuccess);
            return OkResponse<GetAllStudentsResponseDTO>(result, message);
        }
    }
}
