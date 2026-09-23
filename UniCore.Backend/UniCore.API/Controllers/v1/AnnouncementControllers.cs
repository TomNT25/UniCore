using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Announcement.CreateAnnouncement;
using UniCore.Application.Feature.v1.Announcement.DeleteAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetDeliveryReport;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements;
using UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncementWorkflowList;
using UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncements;
using UniCore.Application.Feature.v1.Announcement.GetStudentNewAnnouncements;
using UniCore.Application.Feature.v1.Announcement.MarkAnnouncementAcknowledged;
using UniCore.Application.Feature.v1.Announcement.MarkAnnouncementViewed;
using UniCore.Application.Feature.v1.Announcement.Workflow;
using UniCore.Application.Feature.v1.Announcement.PreviewRecipients;
using UniCore.Application.Feature.v1.Announcement.Targets;
using UniCore.Application.Feature.v1.Announcement.UpdateAnnouncement;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    #region Admin

    /// <summary>
    /// Admin-only announcement management (CRUD, preview, delivery report).
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/admin/announcements")]
    public class AdminAnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;

        public AdminAnnouncementController(IAnnouncementService announcementService, IJsonStringLocalizer localizer)
            : base(localizer)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllAnnouncementResponseDTO>>> GetAll(
            [FromQuery] GetAllAnnouncementRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetAllAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Announcement.GetAllSuccess);
            return OkResponse(result, message);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAnnouncementByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetAnnouncementByIdResponseDTO>>> GetById(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetByIdAsync(id, cancellationToken);
            if (result.Announcement == null)
            {
                return NotFoundResponse<GetAnnouncementByIdResponseDTO>(
                    _localizer.GetString(MessageConstants.Announcement.NotFound));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.GetByIdSuccess));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseAPIResponse<CreateAnnouncementResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreateAnnouncementResponseDTO>>> Create(
            [FromBody] CreateAnnouncementRequestDTO request,
            CancellationToken cancellationToken)
        {
            request.ActorUserId = GetActorUserId();
            var result = await _announcementService.CreateAsync(request, cancellationToken);
            return CreatedResponse(result, _localizer.GetString(MessageConstants.Announcement.CreateSuccess));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateAnnouncementResponseDTO>>> Update(
            string id,
            [FromBody] UpdateAnnouncementRequestDTO request,
            CancellationToken cancellationToken)
        {
            request.Id = id;
            request.ActorUserId = GetActorUserId();
            var result = await _announcementService.UpdateAsync(request, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.UpdateSuccess));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeleteAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeleteAnnouncementResponseDTO>>> Delete(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.DeleteAsync(id, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.DeleteSuccess));
        }

        [HttpPost("preview-recipients")]
        [ProducesResponseType(typeof(BaseAPIResponse<PreviewRecipientsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<PreviewRecipientsResponseDTO>>> PreviewRecipients(
            [FromBody] PreviewRecipientsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.PreviewRecipientsAsync(request, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.PreviewSuccess));
        }

        [HttpGet("{id}/delivery-report")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetDeliveryReportResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetDeliveryReportResponseDTO>>> GetDeliveryReport(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetDeliveryReportAsync(id, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.DeliveryReportSuccess));
        }

        private string? GetActorUserId()
        {
            return User.FindFirst(AuthConstants.Claims.UserId)?.Value
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }

    /// <summary>
    /// Admin autocomplete targets for announcement scope (spec: data + meta, no BaseAPIResponse wrapper).
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/admin/announcements/targets")]
    public class AdminAnnouncementTargetsController : ControllerBase
    {
        private readonly IAnnouncementTargetSearchService _targetSearchService;

        public AdminAnnouncementTargetsController(IAnnouncementTargetSearchService targetSearchService)
        {
            _targetSearchService = targetSearchService;
        }

        [HttpGet("courses")]
        [ProducesResponseType(typeof(AnnouncementTargetSearchResponseDto<CourseTargetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnnouncementTargetSearchResponseDto<CourseTargetItemDto>>> SearchCourses(
            [FromQuery] string? search,
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var result = await _targetSearchService.SearchCoursesAsync(
                new AnnouncementTargetSearchQuery { Search = search, Limit = limit },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("classes")]
        [ProducesResponseType(typeof(AnnouncementTargetSearchResponseDto<ClassTargetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnnouncementTargetSearchResponseDto<ClassTargetItemDto>>> SearchClasses(
            [FromQuery] string? search,
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var result = await _targetSearchService.SearchClassesAsync(
                new AnnouncementTargetSearchQuery { Search = search, Limit = limit },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("departments")]
        [ProducesResponseType(typeof(AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>>> SearchDepartments(
            [FromQuery] string? search,
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var result = await _targetSearchService.SearchDepartmentsAsync(
                new AnnouncementTargetSearchQuery { Search = search, Limit = limit },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("students")]
        [ProducesResponseType(typeof(AnnouncementTargetSearchResponseDto<StudentTargetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnnouncementTargetSearchResponseDto<StudentTargetItemDto>>> SearchStudents(
            [FromQuery] string? search,
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var result = await _targetSearchService.SearchStudentsAsync(
                new AnnouncementTargetSearchQuery { Search = search, Limit = limit },
                cancellationToken);
            return Ok(result);
        }
    }

    #endregion

    #region Student

    /// <summary>
    /// Student announcement endpoints for viewing and interacting with announcements.
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize(Roles = "Student")]
    [Route("api/v{version:apiVersion}/student/announcements")]
    public class StudentAnnouncementController : BaseController
    {
        private readonly GetStudentAnnouncementsHandler _getStudentAnnouncementsHandler;
        private readonly GetStudentAnnouncementWorkflowListHandler _getWorkflowListHandler;
        private readonly GetStudentAnnouncementByIdHandler _getStudentByIdHandler;
        private readonly GetStudentNewAnnouncementsHandler _getNewAnnouncementsHandler;
        private readonly MarkAnnouncementViewedHandler _markViewedHandler;
        private readonly MarkAnnouncementAcknowledgedHandler _markAcknowledgedHandler;

        public StudentAnnouncementController(
            GetStudentAnnouncementsHandler getStudentAnnouncementsHandler,
            GetStudentAnnouncementWorkflowListHandler getWorkflowListHandler,
            GetStudentAnnouncementByIdHandler getStudentByIdHandler,
            GetStudentNewAnnouncementsHandler getNewAnnouncementsHandler,
            MarkAnnouncementViewedHandler markViewedHandler,
            MarkAnnouncementAcknowledgedHandler markAcknowledgedHandler,
            IJsonStringLocalizer localizer)
            : base(localizer)
        {
            _getStudentAnnouncementsHandler = getStudentAnnouncementsHandler;
            _getWorkflowListHandler = getWorkflowListHandler;
            _getStudentByIdHandler = getStudentByIdHandler;
            _getNewAnnouncementsHandler = getNewAnnouncementsHandler;
            _markViewedHandler = markViewedHandler;
            _markAcknowledgedHandler = markAcknowledgedHandler;
        }

        /// <summary>Workflow list (active/expired), no pagination — equivalent to GET .../me?status=</summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(StudentAnnouncementWorkflowListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyWorkflowList(
            [FromQuery] string status = "active",
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();
            var result = await _getWorkflowListHandler.HandleAsync(
                new GetStudentAnnouncementWorkflowListRequest { StudentId = studentId, Status = status },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("get-new")]
        [ProducesResponseType(typeof(StudentAnnouncementWorkflowListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNewAnnouncements(CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();
            var result = await _getNewAnnouncementsHandler.HandleAsync(
                new GetStudentNewAnnouncementsRequest { StudentId = studentId },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("me/{announcementId}")]
        [ProducesResponseType(typeof(StudentAnnouncementWorkflowItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyAnnouncementDetail(
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();
            try
            {
                var result = await _getStudentByIdHandler.HandleAsync(
                    new GetStudentAnnouncementByIdRequest { StudentId = studentId, AnnouncementId = announcementId },
                    cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("confirm-acknowledged")]
        [ProducesResponseType(typeof(MarkAnnouncementAcknowledgedResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmAcknowledged(
            [FromBody] ConfirmAcknowledgedRequestDto body,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();
            var request = new MarkAnnouncementAcknowledgedRequestDTO
            {
                AnnouncementId = body.AnnouncementId,
                StudentId = studentId
            };

            try
            {
                var result = await _markAcknowledgedHandler.HandleAsync(request, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetStudentAnnouncementsResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyAnnouncements(
            [FromQuery] bool? isRead = null,
            [FromQuery] string? type = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            var request = new GetStudentAnnouncementsRequestDTO
            {
                StudentId = studentId,
                IsRead = isRead,
                Type = type,
                PageNumber = page,
                PageSize = Math.Min(pageSize, 100)
            };

            var result = await _getStudentAnnouncementsHandler.HandleAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{announcementId}/view")]
        [ProducesResponseType(typeof(MarkAnnouncementViewedResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsViewed(
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            var request = new MarkAnnouncementViewedRequestDTO
            {
                AnnouncementId = announcementId,
                StudentId = studentId
            };

            try
            {
                var result = await _markViewedHandler.HandleAsync(request, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{announcementId}/acknowledge")]
        [ProducesResponseType(typeof(MarkAnnouncementAcknowledgedResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsAcknowledged(
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            var request = new MarkAnnouncementAcknowledgedRequestDTO
            {
                AnnouncementId = announcementId,
                StudentId = studentId
            };

            try
            {
                var result = await _markAcknowledgedHandler.HandleAsync(request, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        private string GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? User.FindFirst("userId");

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            return userIdClaim.Value;
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// Guest-readable PUBLIC announcements (no authentication).
    /// </summary>
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/public/announcements")]
    public class PublicAnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;

        public PublicAnnouncementController(IAnnouncementService announcementService, IJsonStringLocalizer localizer)
            : base(localizer)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetPublicAnnouncementsResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetPublicAnnouncementsResponseDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetPublicListAsync(cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.GetPublicListSuccess));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetPublicAnnouncementByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetPublicAnnouncementByIdResponseDTO>>> GetById(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetPublicByIdAsync(id, cancellationToken);
            if (result.Announcement == null)
            {
                return NotFoundResponse<GetPublicAnnouncementByIdResponseDTO>(
                    _localizer.GetString(MessageConstants.Announcement.NotFound));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.GetPublicByIdSuccess));
        }
    }

    #endregion
}
