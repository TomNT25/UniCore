using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Auth.Me;
using UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetClassFriends;
using UniCore.Application.Feature.v1.ClassRoom.GetClassInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents;
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
        /// Get Student's Profile
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
        /// Get Class's Info
        /// </summary>
        [Authorize]
        [HttpGet("class")]
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
        /// Get same classmates info
        /// </summary>
        [Authorize]
        [HttpGet("class/classmates")]
        [ProducesResponseType(typeof(BaseAPIResponse<IEnumerable<GetAllStudentsResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<IEnumerable<GetAllStudentsResponseDTO>>>> GetClassmates(
            [FromQuery] string? classId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(classId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return BadRequestResponse<IEnumerable<GetAllStudentsResponseDTO>>(errMessage);
            }

            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
             ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(ClaimTypes.Email)?.Value
             ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<IEnumerable<GetAllStudentsResponseDTO>>(errMessage);
            }

            var input = new GetAllStudentsRequestDTO { ClassID = classId, UserID = userId };

            var result = await _studentService.GetClassmateListAsync(input, ct);
            if (result == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<IEnumerable<GetAllStudentsResponseDTO>>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return OkResponse<IEnumerable<GetAllStudentsResponseDTO>>(result, successMessage);

        }


        /// <summary>
        /// Get course's info
        /// </summary>
        //[Authorize]
        //[HttpGet("course")]
        //[ProducesResponseType(typeof(BaseAPIResponse<IEnumerable<GetCoursesStudentsResponseDTO>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<BaseAPIResponse<IEnumerable<GetCoursesStudentsResponseDTO>>>> GetStudentCoursesInfo(
        //    [FromQuery] string? courseId,
        //    CancellationToken ct = default)
        //{
        //    if (string.IsNullOrEmpty(courseId))
        //    {
        //        var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
        //        return BadRequestResponse<IEnumerable<GetCoursesStudentsResponseDTO>>(errMessage);
        //    }

        //    var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
        //     ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        //     ?? User.FindFirst(ClaimTypes.Email)?.Value
        //     ?? string.Empty;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
        //        return UnauthorizedResponse<IEnumerable<GetCoursesStudentsResponseDTO>>(errMessage);
        //    }

        //    var input = new GetCoursesStudentsRequestDTO { UserID = userId, CourseIDs = new List<string> { courseId } };

        //    var results = await _studentService.GetStudentCoursesAsync(input, ct);
        //    if (results == null)
        //    {
        //        var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
        //        return NotFoundResponse<IEnumerable<GetCoursesStudentsResponseDTO>>(notFoundMessage);
        //    }

        //    var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
        //    return OkResponse<IEnumerable<GetCoursesStudentsResponseDTO>>(results, successMessage);

        //}

        //[Authorize]
        //[HttpGet("class/courses")]
        //[ProducesResponseType(typeof(BaseAPIResponse<IEnumerable<GetClassCourseInfosResponseDTO>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<BaseAPIResponse<IEnumerable<GetClassCourseInfosResponseDTO>>>> GetCoursesInfo(
        //    [FromQuery] string? classId,
        //    CancellationToken ct = default)
        //{
        //    if (string.IsNullOrEmpty(classId))
        //    {
        //        var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
        //        return BadRequestResponse<IEnumerable<GetClassCourseInfosResponseDTO>>(errMessage);
        //    }

        //    var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
        //     ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        //     ?? User.FindFirst(ClaimTypes.Email)?.Value
        //     ?? string.Empty;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
        //        return UnauthorizedResponse<IEnumerable<GetClassCourseInfosResponseDTO>>(errMessage);
        //    }

        //    var input = new GetClassCourseInfosRequestDTO { CourseIds = ["a","b"] };

        //    var results = await _studentService.GetCourseInfosAsync(input, ct);
        //    if (results == null)
        //    {
        //        var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
        //        return NotFoundResponse<IEnumerable<GetClassCourseInfosResponseDTO>>(notFoundMessage);
        //    }

        //    var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
        //    return OkResponse<IEnumerable<GetClassCourseInfosResponseDTO>>(results, successMessage);

        //}
    }
}
