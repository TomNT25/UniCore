using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.FaceAuth.Enroll;
using UniCore.Application.Feature.v1.FaceAuth.GetStatus;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Application.Feature.v1.FaceAuth.SetPin;
using UniCore.Application.Feature.v1.FaceAuth.VerifyPin;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    /// <summary>
    /// Face authentication endpoints — enroll, set PIN, get status, login, verify PIN.
    /// User ID is always taken from JWT token (for authorized endpoints) or face recognition (for login).
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth/face")]
    public class FaceAuthController : BaseController
    {
        private readonly IFaceAuthService _faceAuthService;

        public FaceAuthController(IFaceAuthService faceAuthService, IJsonStringLocalizer localizer)
            : base(localizer)
        {
            _faceAuthService = faceAuthService;
        }

        /// <summary>
        /// Enroll face images (1-5 images for multi-angle enrollment).
        /// </summary>
        /// <remarks>
        /// Upload 1-5 face images for enrollment. For best recognition accuracy,
        /// provide 5 images from different angles:
        /// - face_1: Front view (looking straight)
        /// - face_2: Left turn (~30°)
        /// - face_3: Right turn (~30°)
        /// - face_4: Looking up (~20°)
        /// - face_5: Looking down (~20°)
        /// 
        /// On success, status becomes FACE_PENDING_PIN, requiring PIN setup.
        /// </remarks>
        [Authorize]
        [HttpPost("enroll")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50 MB total
        [ProducesResponseType(typeof(BaseAPIResponse<FaceEnrollResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<FaceEnrollResponseDTO>>> Enroll(
            IFormFile face_1,
            IFormFile? face_2,
            IFormFile? face_3,
            IFormFile? face_4,
            IFormFile? face_5,
            CancellationToken cancellationToken)
        {
            var userId = GetActorUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return UnauthorizedResponse<FaceEnrollResponseDTO>(
                    _localizer.GetString(MessageConstants.Auth.IdentityNotFound));
            }

            if (face_1 == null || face_1.Length == 0)
            {
                return BadRequestResponse<FaceEnrollResponseDTO>(
                    _localizer.GetString(MessageConstants.FaceAuth.ImagesRequired));
            }

            // Collect all provided images
            var faceImages = new List<FaceImageInputDTO>();
            var files = new[] { face_1, face_2, face_3, face_4, face_5 };

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    faceImages.Add(new FaceImageInputDTO
                    {
                        Stream = file.OpenReadStream(),
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Length = file.Length
                    });
                }
            }

            var request = new FaceEnrollRequestDTO
            {
                UserId = userId,
                FaceImages = faceImages
            };

            var result = await _faceAuthService.EnrollAsync(request, cancellationToken);

            if (!result.Success)
            {
                return BadRequestResponse<FaceEnrollResponseDTO>(
                    result.ErrorMessage ?? _localizer.GetString(MessageConstants.FaceAuth.EnrollFailed));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.FaceAuth.EnrollSuccess));
        }

        /// <summary>
        /// Set or update 6-digit PIN for face authentication.
        /// </summary>
        /// <remarks>
        /// Requires face to be enrolled (status FACE_PENDING_PIN or FACE_ENROLLED).
        /// PIN must be exactly 6 digits. On success, status becomes FACE_ENROLLED.
        /// </remarks>
        [Authorize]
        [HttpPost("pin")]
        [ProducesResponseType(typeof(BaseAPIResponse<SetPinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<SetPinResponseDTO>>> SetPin(
            [FromBody] SetPinRequest body,
            CancellationToken cancellationToken)
        {
            var userId = GetActorUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return UnauthorizedResponse<SetPinResponseDTO>(
                    _localizer.GetString(MessageConstants.Auth.IdentityNotFound));
            }

            var request = new SetPinRequestDTO
            {
                UserId = userId,
                Pin = body.Pin,
                ConfirmPin = body.ConfirmPin
            };

            var result = await _faceAuthService.SetPinAsync(request, cancellationToken);

            if (!result.Success)
            {
                return BadRequestResponse<SetPinResponseDTO>(
                    result.ErrorMessage ?? _localizer.GetString(MessageConstants.FaceAuth.SetPinFailed));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.FaceAuth.SetPinSuccess));
        }

        /// <summary>
        /// Get current face authentication status.
        /// </summary>
        /// <remarks>
        /// Returns enrollment status and whether face login is ready.
        /// Possible statuses:
        /// - FACE_NOT_ENROLLED: Face not enrolled
        /// - FACE_PENDING_PIN: Face enrolled, awaiting PIN setup
        /// - FACE_ENROLLED: Ready for face login
        /// - FACE_SUSPENDED: Account suspended
        /// </remarks>
        [Authorize]
        [HttpGet("status")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetFaceStatusResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<GetFaceStatusResponseDTO>>> GetStatus(
            CancellationToken cancellationToken)
        {
            var userId = GetActorUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return UnauthorizedResponse<GetFaceStatusResponseDTO>(
                    _localizer.GetString(MessageConstants.Auth.IdentityNotFound));
            }

            var request = new GetFaceStatusRequestDTO { UserId = userId };
            var result = await _faceAuthService.GetStatusAsync(request, cancellationToken);

            return OkResponse(result, _localizer.GetString(MessageConstants.FaceAuth.GetStatusSuccess));
        }

        /// <summary>
        /// Face login — recognize face and get challenge token.
        /// </summary>
        /// <remarks>
        /// Anonymous endpoint (no JWT required).
        /// Uploads a face image for recognition. On success, returns a short-lived
        /// challenge token (~5 minutes) that must be used with PIN verification.
        /// 
        /// Does NOT return JWT tokens directly — use verify-pin endpoint with
        /// challenge token and PIN to complete login.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("login")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [ProducesResponseType(typeof(BaseAPIResponse<FaceLoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<FaceLoginResponseDTO>>> Login(
            IFormFile? face,
            CancellationToken cancellationToken)
        {
            if (face == null || face.Length == 0)
            {
                return BadRequestResponse<FaceLoginResponseDTO>(
                    _localizer.GetString(MessageConstants.FaceAuth.ImagesRequired));
            }

            await using var stream = face.OpenReadStream();

            var request = new FaceLoginRequestDTO
            {
                FaceStream = stream,
                FileName = face.FileName,
                ContentType = face.ContentType,
                Length = face.Length,
                IpAddress = GetClientIpAddress(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            var result = await _faceAuthService.LoginAsync(request, cancellationToken);

            if (!result.Success)
            {
                return BadRequestResponse<FaceLoginResponseDTO>(
                    result.ErrorMessage ?? _localizer.GetString(MessageConstants.FaceAuth.LoginFailed));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.FaceAuth.LoginSuccess));
        }

        /// <summary>
        /// Verify PIN and complete face login.
        /// </summary>
        /// <remarks>
        /// Anonymous endpoint (no JWT required).
        /// Verifies the 6-digit PIN against the challenge token obtained from
        /// the face login endpoint. On success, returns JWT access and refresh tokens.
        /// 
        /// Challenge token is one-time use and expires after ~5 minutes.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("verify-pin")]
        [ProducesResponseType(typeof(BaseAPIResponse<VerifyPinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<VerifyPinResponseDTO>>> VerifyPin(
            [FromBody] VerifyPinRequest body,
            CancellationToken cancellationToken)
        {
            var request = new VerifyPinRequestDTO
            {
                ChallengeToken = body.ChallengeToken,
                Pin = body.Pin
            };

            var result = await _faceAuthService.VerifyPinAsync(request, cancellationToken);

            if (!result.Success)
            {
                return BadRequestResponse<VerifyPinResponseDTO>(
                    result.ErrorMessage ?? _localizer.GetString(MessageConstants.FaceAuth.VerifyPinFailed));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.FaceAuth.VerifyPinSuccess));
        }

        private string? GetActorUserId()
        {
            return User.FindFirst(AuthConstants.Claims.UserId)?.Value
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst(ClaimTypes.Email)?.Value;
        }

        private string? GetClientIpAddress()
        {
            // Check for forwarded headers (behind proxy/load balancer)
            var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwardedFor))
            {
                return forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()?.Trim();
            }

            // Direct connection IP
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }

    /// <summary>
    /// Request body for setting PIN (during enrollment).
    /// </summary>
    public class SetPinRequest
    {
        /// <summary>
        /// 6-digit PIN.
        /// </summary>
        public string Pin { get; set; } = string.Empty;

        /// <summary>
        /// PIN confirmation (must match Pin).
        /// </summary>
        public string ConfirmPin { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request body for verifying PIN (during face login).
    /// </summary>
    public class VerifyPinRequest
    {
        /// <summary>
        /// Challenge token from face login response.
        /// </summary>
        public string ChallengeToken { get; set; } = string.Empty;

        /// <summary>
        /// 6-digit PIN.
        /// </summary>
        public string Pin { get; set; } = string.Empty;
    }
}
