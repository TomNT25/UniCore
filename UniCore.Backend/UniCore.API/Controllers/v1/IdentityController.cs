using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Identity.GetMyPersonId;
using UniCore.Application.Feature.v1.Identity.ScanCccd;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    /// <summary>
    /// Identity / CCCD verification — OCR proxy + persist VERIFIED to user_person_ids.
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/identity")]
    public class IdentityController : BaseController
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService, IJsonStringLocalizer localizer)
            : base(localizer)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Get current user's CCCD / verification record.
        /// </summary>
        [HttpGet("cccd")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetMyPersonIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<GetMyPersonIdResponseDTO>>> GetMyCccd(
            CancellationToken cancellationToken)
        {
            var userId = GetActorUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return UnauthorizedResponse<GetMyPersonIdResponseDTO>(
                    _localizer.GetString(MessageConstants.Auth.IdentityNotFound));
            }

            var result = await _identityService.GetMyPersonIdAsync(
                new GetMyPersonIdRequestDTO { UserId = userId },
                cancellationToken);

            return OkResponse(result, _localizer.GetString(MessageConstants.Identity.GetCccdSuccess));
        }

        /// <summary>
        /// Scan CCCD via AI OCR, upsert user_person_ids, set VerificationStatus = VERIFIED.
        /// form-data field: image
        /// </summary>
        [HttpPost("cccd/ocr")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [ProducesResponseType(typeof(BaseAPIResponse<ScanCccdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<ScanCccdResponseDTO>>> ScanCccd(
            IFormFile? image,
            CancellationToken cancellationToken)
        {
            var userId = GetActorUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return UnauthorizedResponse<ScanCccdResponseDTO>(
                    _localizer.GetString(MessageConstants.Auth.IdentityNotFound));
            }

            if (image == null || image.Length == 0)
            {
                return BadRequestResponse<ScanCccdResponseDTO>(
                    _localizer.GetString(MessageConstants.Identity.ImageRequired));
            }

            await using var stream = image.OpenReadStream();

            var result = await _identityService.ScanCccdAsync(
                new ScanCccdRequestDTO
                {
                    UserId = userId,
                    ImageStream = stream,
                    FileName = image.FileName,
                    ContentType = image.ContentType,
                    FileLength = image.Length
                },
                cancellationToken);

            return OkResponse(result, _localizer.GetString(MessageConstants.Identity.ScanCccdSuccess));
        }

        private string? GetActorUserId()
        {
            return User.FindFirst(AuthConstants.Claims.UserId)?.Value
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
