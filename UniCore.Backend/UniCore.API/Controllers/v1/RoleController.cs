using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Role.CreateRole;
using UniCore.Application.Feature.v1.Role.DeleteRole;
using UniCore.Application.Feature.v1.Role.GetAllRole;
using UniCore.Application.Feature.v1.Role.GetRoleById;
using UniCore.Application.Feature.v1.Role.GrantRolePermission;
using UniCore.Application.Feature.v1.Role.GrantUserRole;
using UniCore.Application.Feature.v1.Role.RevokeRolePermission;
using UniCore.Application.Feature.v1.Role.RevokeUserRole;
using UniCore.Application.Feature.v1.Role.UpdateRole;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/roles")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllRoleResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllRoleResponseDTO>>> GetAll([FromQuery] GetAllRoleRequestDTO request)
        {
            var result = await _roleService.GetAllAsync(request);
            var message = _localizer.GetString(MessageConstants.Role.GetAllSuccess);
            return OkResponse<GetAllRoleResponseDTO>(result, message);
        }

        /// <summary>
        /// Get role by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetRoleByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetRoleByIdResponseDTO>>> GetById(string id)
        {
            var result = await _roleService.GetByIdAsync(id);
            var message = _localizer.GetString(MessageConstants.Role.NotFound);
            if (result.Role == null)
            {
                return NotFoundResponse<GetRoleByIdResponseDTO>(message);
            }

            message = _localizer.GetString(MessageConstants.Role.GetByIdSuccess);
            return OkResponse<GetRoleByIdResponseDTO>(result, message);
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseAPIResponse<CreateRoleResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreateRoleResponseDTO>>> Create([FromBody] CreateRoleRequestDTO request)
        {
            var result = await _roleService.CreateAsync(request);
            var message = _localizer.GetString(MessageConstants.Role.CreateSuccess);
            return CreatedResponse<CreateRoleResponseDTO>(result, message);
        }

        /// <summary>
        /// Update an existing role
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateRoleResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateRoleResponseDTO>>> Update(string id, [FromBody] UpdateRoleRequestDTO request)
        {
            request.Id = id;
            var result = await _roleService.UpdateAsync(request);
            var message = _localizer.GetString(MessageConstants.Role.UpdateSuccess);
            return OkResponse<UpdateRoleResponseDTO>(result, message);
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeleteRoleResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeleteRoleResponseDTO>>> Delete(string id)
        {
            var result = await _roleService.DeleteAsync(id);
            var message = _localizer.GetString(MessageConstants.Role.DeleteSuccess);
            return OkResponse<DeleteRoleResponseDTO>(result, message);
        }

        /// <summary>
        /// Grant permissions to a role
        /// </summary>
        [HttpPost("grant-permission")]
        [ProducesResponseType(typeof(BaseAPIResponse<GrantRolePermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<GrantRolePermissionResponseDTO>>> GrantPermission([FromBody] GrantRolePermissionRequestDTO request)
        {
            var result = await _roleService.GrantRolePermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Role.GrantPermissionSuccess);
            return OkResponse<GrantRolePermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Revoke permissions from a role
        /// </summary>
        [HttpPost("revoke-permission")]
        [ProducesResponseType(typeof(BaseAPIResponse<RevokeRolePermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<RevokeRolePermissionResponseDTO>>> RevokePermission([FromBody] RevokeRolePermissionRequestDTO request)
        {
            var result = await _roleService.RevokeRolePermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Role.RevokePermissionSuccess);
            return OkResponse<RevokeRolePermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Grant role(s) to a user
        /// </summary>
        [HttpPost("grant-user")]
        [ProducesResponseType(typeof(BaseAPIResponse<GrantUserRoleResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<GrantUserRoleResponseDTO>>> GrantUserRole([FromBody] GrantUserRoleRequestDTO request)
        {
            var result = await _roleService.GrantUserRoleAsync(request);
            var message = _localizer.GetString(MessageConstants.Role.GrantUserSuccess);
            return OkResponse<GrantUserRoleResponseDTO>(result, message);
        }

        /// <summary>
        /// Revoke role(s) from a user
        /// </summary>
        [HttpPost("revoke-user")]
        [ProducesResponseType(typeof(BaseAPIResponse<RevokeUserRoleResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<RevokeUserRoleResponseDTO>>> RevokeUserRole([FromBody] RevokeUserRoleRequestDTO request)
        {
            var result = await _roleService.RevokeUserRoleAsync(request);
            var message = _localizer.GetString(MessageConstants.Role.RevokeUserSuccess);
            return OkResponse<RevokeUserRoleResponseDTO>(result, message);
        }
    }
}
