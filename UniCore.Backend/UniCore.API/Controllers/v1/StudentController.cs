using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Auth.Me;
using UniCore.Application.Feature.v1.Classes.GetClasses;
using UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetClassFriends;
using UniCore.Application.Feature.v1.ClassRoom.GetClassInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents;
using UniCore.Application.Feature.v1.Courses.GetAllCourses;
using UniCore.Application.Feature.v1.Role.GetAllRole;
using UniCore.Application.Feature.v1.User.GetUserInfo;
using UniCore.Application.Service.v1;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/student")]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Get Student's Profile, using UserId
        /// </summary>
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetUserInfoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetUserInfoResponseDTO>>> GetMe(CancellationToken ct)
        {
            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
                         ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(ClaimTypes.Email)?.Value
                         ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetUserInfoResponseDTO>(errMessage);
            }

            GetUserInfoRequestDTO input = new GetUserInfoRequestDTO{ UserID = userId };

            var result = await _studentService.GetUserInfoAsync(input, ct);
            if (result == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<GetUserInfoResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return OkResponse<GetUserInfoResponseDTO>(result, successMessage);
        }


        /// <summary>
        /// Get Class's Info, using UserId
        /// </summary>
        [Authorize]
        [HttpGet("my-class")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetClassInfoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetClassInfoResponseDTO>>> GetClassInfo(
            [FromQuery] string? classId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(classId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return BadRequestResponse<GetClassInfoResponseDTO>(errMessage);
            }

            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
             ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(ClaimTypes.Email)?.Value
             ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetClassInfoResponseDTO>(errMessage);
            }

            var input = new GetClassInfoRequestDTO { ClassID = classId };

            var result = await _studentService.GetClassInfoAsync(input, ct);

            if (result == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<GetClassInfoResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return OkResponse<GetClassInfoResponseDTO>(result, successMessage);


        }

        /// <summary>
        /// Get same classmates info, using UserId
        /// </summary>
        [Authorize]
        [HttpGet("my-class/classmates")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetClassFriendsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetClassFriendsResponseDTO>>> GetClassmates(
            [FromQuery] string? classId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(classId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return BadRequestResponse<GetClassFriendsResponseDTO>(errMessage);
            }

            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
             ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(ClaimTypes.Email)?.Value
             ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetClassFriendsResponseDTO>(errMessage);
            }

            var input = new GetClassFriendsRequestDTO { ClassID = classId, UserID = userId };

            var result = await _studentService.GetClassmateListAsync(input, ct);
            if (result == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<GetClassFriendsResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return OkResponse<GetClassFriendsResponseDTO>(result, successMessage);

        }


        /// <summary>
        /// Get Student course's personal info, using UserId
        /// </summary>
        [HttpGet("my-courses")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetCoursesStudentsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetCoursesStudentsResponseDTO>>> GetStudentCourses(
            CancellationToken ct = default)
        {

            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
             ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(ClaimTypes.Email)?.Value
             ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetCoursesStudentsResponseDTO>(errMessage);
            }

            var input = new GetCoursesStudentsRequestDTO { UserID = userId };

            var results = await _studentService.GetStudentCoursesAsync(input, ct);
            if (results is null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<GetCoursesStudentsResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString("Success");
            return OkResponse<GetCoursesStudentsResponseDTO>(results, successMessage);

        }


        /// <summary>
        /// Get Student course's public info, using UserId
        /// </summary>
        [Authorize]
        [HttpGet("my-class/courses")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetClassCourseInfosResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetClassCourseInfosResponseDTO>>> GetCoursesInfo(
            [FromQuery] string? classId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(classId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return BadRequestResponse<GetClassCourseInfosResponseDTO>(errMessage);
            }

            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
             ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(ClaimTypes.Email)?.Value
             ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetClassCourseInfosResponseDTO>(errMessage);
            }

            var input = new GetClassCourseInfosRequestDTO { UserID = userId };

            var results = await _studentService.GetCourseInfosAsync(input, ct);
            if (results == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<GetClassCourseInfosResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return OkResponse<GetClassCourseInfosResponseDTO>(results, successMessage);


        }

        /// <summary>
        /// Get all Public Courses
        /// </summary>
        [Authorize]
        [HttpGet("courses")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllCoursesResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllCoursesResponseDTO>>> GetAllCourses([FromQuery] GetAllCoursesRequestDTO request)
        {
            var result = await _studentService.GetCoursesByName(request);
            var message = _localizer.GetString(MessageConstants.Role.GetAllSuccess);
            return OkResponse<GetAllCoursesResponseDTO>(result, message);
        }

        /// <summary>
        /// Get all Public Classes
        /// </summary>
        [Authorize]
        [HttpGet("classes")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetClassesResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetClassesResponseDTO>>> GetAllClassesByName([FromQuery] GetClassesRequestDTO request)
        {
            var result = await _studentService.GetClassesByName(request);
            var message = _localizer.GetString(MessageConstants.Role.GetAllSuccess);
            return OkResponse<GetClassesResponseDTO>(result, message);
        }



    }
}
