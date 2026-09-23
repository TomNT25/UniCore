using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Permission.GrantUserPermission;
using UniCore.Application.Feature.v1.Permission.RevokeUserPermission;
using UniCore.Application.Feature.v1.Rbac.GetRbacOverview;
using UniCore.Application.Feature.v1.Rbac.GetUserAccessMatrix;
using UniCore.Application.Feature.v1.Role.GrantRolePermission;
using UniCore.Application.Feature.v1.Role.GrantUserRole;
using UniCore.Application.Feature.v1.Role.RevokeRolePermission;
using UniCore.Application.Feature.v1.Role.RevokeUserRole;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/rbac")]
    public class RbacController : BaseController
    {
        private readonly IRbacService _rbacService;

        public RbacController(IRbacService rbacService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _rbacService = rbacService;
        }

        /// <summary>
        /// Get RBAC overview metrics and summary per role
        /// </summary>
        [HttpGet("overview")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetRbacOverviewResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetRbacOverviewResponseDTO>>> GetOverview([FromQuery] GetRbacOverviewRequestDTO request)
        {
            var result = await _rbacService.GetOverviewAsync(request);
            var message = _localizer.GetString(MessageConstants.Rbac.GetOverviewSuccess);
            return OkResponse<GetRbacOverviewResponseDTO>(result, message);
        }

        /// <summary>
        /// Get user access matrix detailing assigned roles, direct permissions, and effective permission set
        /// </summary>
        [HttpGet("users/{userId}/access-matrix")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetUserAccessMatrixResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetUserAccessMatrixResponseDTO>>> GetUserAccessMatrix(string userId)
        {
            var request = new GetUserAccessMatrixRequestDTO { UserId = userId };
            var result = await _rbacService.GetUserAccessMatrixAsync(request);

            if (string.IsNullOrEmpty(result.UserId))
            {
                var notFoundMsg = _localizer.GetString(MessageConstants.Rbac.UserNotFound);
                return NotFoundResponse<GetUserAccessMatrixResponseDTO>(notFoundMsg);
            }

            var message = _localizer.GetString(MessageConstants.Rbac.GetUserAccessMatrixSuccess);
            return OkResponse<GetUserAccessMatrixResponseDTO>(result, message);
        }

        /// <summary>
        /// Grant secondary role(s) to a user
        /// </summary>
        [HttpPost("users/grant-role")]
        [ProducesResponseType(typeof(BaseAPIResponse<GrantUserRoleResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<GrantUserRoleResponseDTO>>> GrantUserRole([FromBody] GrantUserRoleRequestDTO request)
        {
            var result = await _rbacService.GrantUserRoleAsync(request);
            var message = _localizer.GetString(MessageConstants.Rbac.GrantUserRoleSuccess);
            return OkResponse<GrantUserRoleResponseDTO>(result, message);
        }

        /// <summary>
        /// Revoke secondary role(s) from a user
        /// </summary>
        [HttpPost("users/revoke-role")]
        [ProducesResponseType(typeof(BaseAPIResponse<RevokeUserRoleResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<RevokeUserRoleResponseDTO>>> RevokeUserRole([FromBody] RevokeUserRoleRequestDTO request)
        {
            var result = await _rbacService.RevokeUserRoleAsync(request);
            var message = _localizer.GetString(MessageConstants.Rbac.RevokeUserRoleSuccess);
            return OkResponse<RevokeUserRoleResponseDTO>(result, message);
        }

        /// <summary>
        /// Grant direct permission(s) to a user
        /// </summary>
        [HttpPost("users/grant-permission")]
        [ProducesResponseType(typeof(BaseAPIResponse<GrantUserPermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<GrantUserPermissionResponseDTO>>> GrantUserPermission([FromBody] GrantUserPermissionRequestDTO request)
        {
            var result = await _rbacService.GrantUserPermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Rbac.GrantUserPermissionSuccess);
            return OkResponse<GrantUserPermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Revoke direct permission(s) from a user
        /// </summary>
        [HttpPost("users/revoke-permission")]
        [ProducesResponseType(typeof(BaseAPIResponse<RevokeUserPermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<RevokeUserPermissionResponseDTO>>> RevokeUserPermission([FromBody] RevokeUserPermissionRequestDTO request)
        {
            var result = await _rbacService.RevokeUserPermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Rbac.RevokeUserPermissionSuccess);
            return OkResponse<RevokeUserPermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Grant permission(s) to a role
        /// </summary>
        [HttpPost("roles/grant-permission")]
        [ProducesResponseType(typeof(BaseAPIResponse<GrantRolePermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<GrantRolePermissionResponseDTO>>> GrantRolePermission([FromBody] GrantRolePermissionRequestDTO request)
        {
            var result = await _rbacService.GrantRolePermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Rbac.GrantRolePermissionSuccess);
            return OkResponse<GrantRolePermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Revoke permission(s) from a role
        /// </summary>
        [HttpPost("roles/revoke-permission")]
        [ProducesResponseType(typeof(BaseAPIResponse<RevokeRolePermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<RevokeRolePermissionResponseDTO>>> RevokeRolePermission([FromBody] RevokeRolePermissionRequestDTO request)
        {
            var result = await _rbacService.RevokeRolePermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Rbac.RevokeRolePermissionSuccess);
            return OkResponse<RevokeRolePermissionResponseDTO>(result, message);
        }
    }
}
