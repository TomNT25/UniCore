using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Auth.Me;
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

    }
}
