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
using UniCore.Application.Feature.v1.Courses.GetMyCourses.ListDetails;
using UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails;
using UniCore.Application.Feature.v1.Role.GetAllRole;
using UniCore.Application.Feature.v1.User.GetUserInfo;
using UniCore.Application.Feature.v1.User.PostUserInfo;
using UniCore.Application.Feature.v1.User.PutUserInfo;
using UniCore.Application.Service.v1;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = AuthConstants.Roles.Student)]
    [Route("api/v{version:apiVersion}/student")]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Put Student's Profile, using UserId
        /// </summary>
        [Authorize]
        [HttpPut("profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<PutUserInfoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<PutUserInfoResponseDTO>>> PutUserProfile
            ([FromBody] PutUserInfoDTO bio,CancellationToken ct)
        {
            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
                         ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(ClaimTypes.Email)?.Value
                         ?? string.Empty;


            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<PutUserInfoResponseDTO>(errMessage);
            }

            PutUserInfoRequestDTO input = new PutUserInfoRequestDTO{ UserID = userId, PutUserBio = bio };

            var result = await _studentService.PutUserInfoAsync(input, ct);
            if (result == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<PutUserInfoResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return OkResponse<PutUserInfoResponseDTO>(result, successMessage);
        }

        /// <summary>
        /// Post Student's Profile, using UserId
        /// </summary>
        [Authorize]
        [HttpPost("profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<PostUserInfoResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<PostUserInfoResponseDTO>>> PostUserProfile
            ([FromBody] PostUserInfoRequestDTO request, CancellationToken ct)
        {
            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
                         ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(ClaimTypes.Email)?.Value
                         ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<PostUserInfoResponseDTO>(errMessage);
            }

            request.UserId = userId;

            var result = await _studentService.PostUserInfoAsync(request, ct);
            if (result == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<PostUserInfoResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return CreatedResponse<PostUserInfoResponseDTO>(result, successMessage);
        }

        /// <summary>
        /// Get Student's Profile, using UserId
        /// </summary>
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetUserInfoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetUserInfoResponseDTO>>> GetStudentProfile(CancellationToken ct)
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
                var errMessage = _localizer.GetString(MessageConstants.System.BadRequest);
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
                var notFoundMessage = _localizer.GetString(MessageConstants.Student.GetInfoFailed);
                return NotFoundResponse<GetClassInfoResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Student.GetInfoSuccess);
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
                var notFoundMessage = _localizer.GetString(MessageConstants.Student.GetInfoFailed);
                return NotFoundResponse<GetClassFriendsResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Student.GetInfoSuccess);
            return OkResponse<GetClassFriendsResponseDTO>(result, successMessage);

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


        /// <summary>
        /// Get All Student's Courses, using UserId
        /// </summary>
        [Authorize]
        [HttpGet("courses/me")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetMyCoursesResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetMyCoursesResponseDTO>>> GetMyCourses([FromQuery] PageNumberPaginationRequest request)
        {
            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
             ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(ClaimTypes.Email)?.Value
             ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetMyCoursesResponseDTO>(errMessage);
            }

            var input = new GetMyCoursesRequestDTO { UserID = userId };


            var result = await _studentService.GetMyCoursesAsync(input);
            if (result is null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Student.GetMyCoursesEmpty);
                return NotFoundResponse<GetMyCoursesResponseDTO>(notFoundMessage);
            }

            var message = _localizer.GetString(MessageConstants.Student.GetMyCoursesSuccess);
            return OkResponse<GetMyCoursesResponseDTO>(result, message);
        }


        /// <summary>
        /// Get Student's Course Details Info, using UserId
        /// </summary>
        [Authorize]
        [HttpGet("courses/{courseId}/me")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetCourseDetailsResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetCourseDetailsResponseDTO>>> GetCourseDetails(
            string courseId)
        {
            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
             ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(ClaimTypes.Email)?.Value
             ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetCourseDetailsResponseDTO>(errMessage);
            }

            var input = new GetCourseDetailsRequestDTO { UserID = userId, CourseID = courseId };


            var result = await _studentService.GetCourseDetailsAsync(input);

            if (result.Courses is null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Student.GetCourseDetailsNotFound);
                return NotFoundResponse<GetCourseDetailsResponseDTO>(notFoundMessage);
            }

            var message = _localizer.GetString(MessageConstants.Student.GetCourseDetailsSuccess);
            return OkResponse<GetCourseDetailsResponseDTO>(result, message);
        }
    }
}

