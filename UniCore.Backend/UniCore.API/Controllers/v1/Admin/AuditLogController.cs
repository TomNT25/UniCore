using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.CreateAuditLog;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.DeleteAuditLog;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAllAuditLogs;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.GetAuditLogById;
using UniCore.Application.Feature.v1.Admin.AuditLogsManagement.UpdateAuditLog;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1.Admin
{
    [ApiVersion("1.0")]
    [Authorize(Roles = $"{DatabaseConstants.Roles.AdminName},{DatabaseConstants.Roles.AdminCode}")]
    [Route("api/v{version:apiVersion}/admin/audit-logs")]
    [Route("api/v{version:apiVersion}/admin/app-logs")]
    public class AuditLogController : BaseController
    {
        private readonly IAdminService _adminService;

        public AuditLogController(IAdminService adminService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _adminService = adminService;
        }

        // ==========================================
        // 1. AUDIT LOG MANAGEMENT (app_logs)
        // ==========================================

        /// <summary>
        /// Get paged audit logs with filters (log level, date range, trace ID, search term)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllAuditLogsResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllAuditLogsResponseDTO>>> GetAllAuditLogs([FromQuery] GetAllAuditLogsRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetAllAuditLogsAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.GetAllAuditLogsSuccess);
            return OkResponse<GetAllAuditLogsResponseDTO>(result, message);
        }

        /// <summary>
        /// Get audit log by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAuditLogByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetAuditLogByIdResponseDTO>>> GetAuditLogById(string id, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetAuditLogByIdAsync(id, cancellationToken);
            if (result.Log == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Admin.AuditLogNotFound);
                return NotFoundResponse<GetAuditLogByIdResponseDTO>(notFoundMessage);
            }

            var message = _localizer.GetString(MessageConstants.Admin.GetAuditLogByIdSuccess);
            return OkResponse<GetAuditLogByIdResponseDTO>(result, message);
        }

        /// <summary>
        /// Create new audit log entry
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseAPIResponse<CreateAuditLogResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreateAuditLogResponseDTO>>> CreateAuditLog([FromBody] CreateAuditLogRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.CreateAuditLogAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.CreateAuditLogSuccess);
            return CreatedResponse<CreateAuditLogResponseDTO>(result, message);
        }

        /// <summary>
        /// Update audit log entry
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateAuditLogResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateAuditLogResponseDTO>>> UpdateAuditLog(string id, [FromBody] UpdateAuditLogRequestDTO request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _adminService.UpdateAuditLogAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateAuditLogSuccess);
            return OkResponse<UpdateAuditLogResponseDTO>(result, message);
        }

        /// <summary>
        /// Delete audit log entry
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeleteAuditLogResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeleteAuditLogResponseDTO>>> DeleteAuditLog(string id, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.DeleteAuditLogAsync(id, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.DeleteAuditLogSuccess);
            return OkResponse<DeleteAuditLogResponseDTO>(result, message);
        }
    }
}
