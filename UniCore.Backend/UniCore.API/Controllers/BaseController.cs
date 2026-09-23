using Microsoft.AspNetCore.Mvc;
using UniCore.Application.DTO;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IJsonStringLocalizer _localizer;

        public BaseController(IJsonStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        // ==========================================
        // 2xx SUCCESS RESPONSES
        // ==========================================

        protected ActionResult<BaseAPIResponse<T>> OkResponse<T>(T data, string? message = null)
        {
            return StatusCode(StatusCodes.Status200OK,
                BaseAPIResponse<T>.Success(data, message, StatusCodes.Status200OK));
        }

        protected ActionResult<BaseAPIResponse<T>> CreatedResponse<T>(T data, string? message = null)
        {
            return StatusCode(StatusCodes.Status201Created,
                BaseAPIResponse<T>.Success(data, message, StatusCodes.Status201Created));
        }

        // ==========================================
        // 4xx CLIENT ERROR RESPONSES
        // ==========================================

        protected ActionResult<BaseAPIResponse<T>> BadRequestResponse<T>(string message, List<string>? errors = null)
        {
            return StatusCode(StatusCodes.Status400BadRequest,
                BaseAPIResponse<T>.Failure(message, StatusCodes.Status400BadRequest, errors));
        }

        protected ActionResult<BaseAPIResponse<T>> NotFoundResponse<T>(string message)
        {
            return StatusCode(StatusCodes.Status404NotFound,
                BaseAPIResponse<T>.Failure(message, StatusCodes.Status404NotFound));
        }

        protected ActionResult<BaseAPIResponse<T>> UnauthorizedResponse<T>(string message)
        {
            return StatusCode(StatusCodes.Status401Unauthorized,
                BaseAPIResponse<T>.Failure(message, StatusCodes.Status401Unauthorized));
        }

        protected ActionResult<BaseAPIResponse<T>> ForbiddenResponse<T>(string message)
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                BaseAPIResponse<T>.Failure(message, StatusCodes.Status403Forbidden));
        }
    }
}