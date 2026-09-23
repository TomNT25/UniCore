using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Permission.CreatePermission;
using UniCore.Application.Feature.v1.Permission.DeletePermission;
using UniCore.Application.Feature.v1.Permission.GetAllPermission;
using UniCore.Application.Feature.v1.Permission.GetPermissionById;
using UniCore.Application.Feature.v1.Permission.GrantUserPermission;
using UniCore.Application.Feature.v1.Permission.RevokeUserPermission;
using UniCore.Application.Feature.v1.Permission.UpdatePermission;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/permissions")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Get all permissions
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllPermissionResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllPermissionResponseDTO>>> GetAll([FromQuery] GetAllPermissionRequestDTO request)
        {
            var result = await _permissionService.GetAllAsync(request);
            var message = _localizer.GetString(MessageConstants.Permission.GetAllSuccess);
            return OkResponse<GetAllPermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Get permission by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetPermissionByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetPermissionByIdResponseDTO>>> GetById(string id)
        {
            var result = await _permissionService.GetByIdAsync(id);
            var message = _localizer.GetString(MessageConstants.Permission.NotFound);

            if (result.Permission == null)
            {
                return NotFoundResponse<GetPermissionByIdResponseDTO>(message);
            }

            message = _localizer.GetString(MessageConstants.Permission.GetByIdSuccess);
            return OkResponse<GetPermissionByIdResponseDTO>(result, message);
        }

        /// <summary>
        /// Create a new permission
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseAPIResponse<CreatePermissionResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreatePermissionResponseDTO>>> Create([FromBody] CreatePermissionRequestDTO request)
        {
            var result = await _permissionService.CreateAsync(request);
            var message = _localizer.GetString(MessageConstants.Permission.CreateSuccess);
            return CreatedResponse<CreatePermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Update an existing permission
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdatePermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdatePermissionResponseDTO>>> Update(string id, [FromBody] UpdatePermissionRequestDTO request)
        {
            request.Id = id;
            var result = await _permissionService.UpdateAsync(request);
            var message = _localizer.GetString(MessageConstants.Permission.UpdateSuccess);
            return OkResponse<UpdatePermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Delete a permission
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeletePermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeletePermissionResponseDTO>>> Delete(string id)
        {
            var result = await _permissionService.DeleteAsync(id);
            var message = _localizer.GetString(MessageConstants.Permission.DeleteSuccess);
            return OkResponse<DeletePermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Grant permission(s) directly to a user
        /// </summary>
        [HttpPost("grant-user")]
        [ProducesResponseType(typeof(BaseAPIResponse<GrantUserPermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<GrantUserPermissionResponseDTO>>> GrantUserPermission([FromBody] GrantUserPermissionRequestDTO request)
        {
            var result = await _permissionService.GrantUserPermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Permission.GrantUserSuccess);
            return OkResponse<GrantUserPermissionResponseDTO>(result, message);
        }

        /// <summary>
        /// Revoke direct permission(s) from a user
        /// </summary>
        [HttpPost("revoke-user")]
        [ProducesResponseType(typeof(BaseAPIResponse<RevokeUserPermissionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<RevokeUserPermissionResponseDTO>>> RevokeUserPermission([FromBody] RevokeUserPermissionRequestDTO request)
        {
            var result = await _permissionService.RevokeUserPermissionAsync(request);
            var message = _localizer.GetString(MessageConstants.Permission.RevokeUserSuccess);
            return OkResponse<RevokeUserPermissionResponseDTO>(result, message);
        }
    }
}
