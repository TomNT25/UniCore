using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1.Admin
{
    [ApiVersion("1.0")]
    [Authorize(Roles = $"{DatabaseConstants.Roles.AdminName},{DatabaseConstants.Roles.AdminCode}")]
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
        public async Task<ActionResult<BaseAPIResponse<GetDashboardOverviewResponseDTO>>> GetDashboardOverview([FromQuery] GetDashboardOverviewRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetDashboardOverviewAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.GetDashboardSuccess);
            return OkResponse<GetDashboardOverviewResponseDTO>(result, message);
        }
    }
}
