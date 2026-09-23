using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using UniCore.Application.DTO;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Extensions
{
    public class GlobalExceptionExtension : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionExtension> _logger;
        private readonly IHostEnvironment _env;
        private readonly IJsonStringLocalizer _localizer;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public GlobalExceptionExtension(
            ILogger<GlobalExceptionExtension> logger,
            IHostEnvironment env,
            IJsonStringLocalizer localizer)
        {
            _logger = logger;
            _env = env;
            _localizer = localizer;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, ExceptionConstants.UnhandledExceptionUniCore, exception.Message);

            if (httpContext.Response.HasStarted)
            {
                _logger.LogWarning("Response has already started, cannot write exception response.");
                return false;
            }

            httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            (int statusCode, string messageKey, string? customMessage, List<string>? errors) = exception switch
            {
                ValidationException valEx => (
                    StatusCodes.Status400BadRequest,
                    MessageConstants.System.ValidationFailed,
                    (string?)null,
                    (List<string>?)valEx.Errors.Select(e => _localizer.GetString(e.ErrorMessage)).ToList()
                ),
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    MessageConstants.System.UnauthorizedAccess,
                    (string?)_localizer.GetString(exception.Message),
                    (List<string>?)null
                ),
                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    MessageConstants.System.ResourceNotFound,
                    (string?)_localizer.GetString(exception.Message),
                    (List<string>?)null
                ),
                ArgumentException or InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    MessageConstants.System.ValidationFailed,
                    (string?)_localizer.GetString(exception.Message),
                    (List<string>?)null
                ),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    MessageConstants.System.UnexpectedError,
                    (string?)(_env.IsDevelopment() ? exception.Message : _localizer.GetString(MessageConstants.System.UnexpectedError)),
                    (List<string>?)null
                )
            };

            var finalMessage = customMessage ?? _localizer.GetString(messageKey);

            httpContext.Response.StatusCode = statusCode;

            var response = BaseAPIResponse<object>.Failure(finalMessage, statusCode, errors);

            await JsonSerializer.SerializeAsync(httpContext.Response.Body, response, _jsonOptions, cancellationToken);

            return true;
        }
    }
}