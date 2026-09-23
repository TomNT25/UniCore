using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.CreateCourse;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.DeleteCourse;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.GetAllCourses;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.GetCourseById;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourse;
using UniCore.Application.Feature.v1.Admin.CoursesManagement.UpdateCourseStatus;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1.Admin
{
    [ApiVersion("1.0")]
    [Authorize(Roles = $"{DatabaseConstants.Roles.AdminName},{DatabaseConstants.Roles.AdminCode}")]
    [Route("api/v{version:apiVersion}/admin/courses")]
    public class CourseController : BaseController
    {
        private readonly IAdminService _adminService;

        public CourseController(IAdminService adminService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _adminService = adminService;
        }

        // ==========================================
        // 1. COURSE MANAGEMENT
        // ==========================================

        /// <summary>
        /// Get paged courses list
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllCoursesResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllCoursesResponseDTO>>> GetAllCourses([FromQuery] GetAllCoursesRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetAllCoursesAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.GetAllCoursesSuccess);
            return OkResponse<GetAllCoursesResponseDTO>(result, message);
        }

        /// <summary>
        /// Get course by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetCourseByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetCourseByIdResponseDTO>>> GetCourseById(string id, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.GetCourseByIdAsync(id, cancellationToken);
            if (result.Course == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Admin.CourseNotFound);
                return NotFoundResponse<GetCourseByIdResponseDTO>(notFoundMessage);
            }

            var message = _localizer.GetString(MessageConstants.Admin.GetCourseByIdSuccess);
            return OkResponse<GetCourseByIdResponseDTO>(result, message);
        }

        /// <summary>
        /// Create new course
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseAPIResponse<CreateCourseResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreateCourseResponseDTO>>> CreateCourse([FromBody] CreateCourseRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.CreateCourseAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.CreateCourseSuccess);
            return CreatedResponse<CreateCourseResponseDTO>(result, message);
        }

        /// <summary>
        /// Update course information
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateCourseResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateCourseResponseDTO>>> UpdateCourse(string id, [FromBody] UpdateCourseRequestDTO request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _adminService.UpdateCourseAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateCourseSuccess);
            return OkResponse<UpdateCourseResponseDTO>(result, message);
        }

        /// <summary>
        /// Delete course
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeleteCourseResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeleteCourseResponseDTO>>> DeleteCourse(string id, CancellationToken cancellationToken = default)
        {
            var result = await _adminService.DeleteCourseAsync(id, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.DeleteCourseSuccess);
            return OkResponse<DeleteCourseResponseDTO>(result, message);
        }

        /// <summary>
        /// Update course active status
        /// </summary>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateCourseStatusResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateCourseStatusResponseDTO>>> UpdateCourseStatus(string id, [FromBody] UpdateCourseStatusRequestDTO request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _adminService.UpdateCourseStatusAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Admin.UpdateCourseStatusSuccess);
            return OkResponse<UpdateCourseStatusResponseDTO>(result, message);
        }
    }
}
