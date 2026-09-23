using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Entity;

namespace UniCore.Application.Service.v1
{
    public class FaceAuthAuditService : IFaceAuthAuditService
    {
        private readonly IFaceAuthLogRepository _logRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FaceAuthAuditService> _logger;

        // Default rate limit settings
        private const int DefaultMaxLoginAttemptsPerIpPerMinute = 10;
        private const int DefaultMaxLoginAttemptsPerUserPerMinute = 5;
        private const int DefaultRateLimitWindowMinutes = 1;

        public FaceAuthAuditService(
            IFaceAuthLogRepository logRepository,
            IConfiguration configuration,
            ILogger<FaceAuthAuditService> logger)
        {
            _logRepository = logRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task LogAsync(FaceAuthAuditEntry entry, CancellationToken cancellationToken = default)
        {
            try
            {
                var log = new FaceAuthLog
                {
                    RequestId = entry.RequestId,
                    UserId = entry.UserId,
                    Action = entry.Action.ToString().ToUpperInvariant(),
                    Result = entry.Result.ToString().ToUpperInvariant(),
                    ErrorCode = entry.ErrorCode,
                    ModelVersion = entry.ModelVersion,
                    Similarity = entry.Similarity,
                    LatencyMs = entry.LatencyMs,
                    IpAddress = entry.IpAddress,
                    UserAgent = TruncateUserAgent(entry.UserAgent),
                    Metadata = entry.Metadata != null
                        ? JsonSerializer.Serialize(entry.Metadata)
                        : null
                };

                await _logRepository.AddLogAsync(log, cancellationToken);

                _logger.LogDebug(
                    "Face auth audit: {Action} {Result} for user {UserId} from {IpAddress}",
                    log.Action, log.Result, log.UserId ?? "anonymous", log.IpAddress ?? "unknown");
            }
            catch (Exception ex)
            {
                // Don't let audit logging failures break the main flow
                _logger.LogError(ex,
                    "Failed to write face auth audit log for request {RequestId}",
                    entry.RequestId);
            }
        }

        public async Task<RateLimitResult> CheckIpRateLimitAsync(
            string ipAddress,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return new RateLimitResult { IsLimited = false };
            }

            var maxAttempts = GetConfigInt("FaceAi:RateLimit:MaxLoginAttemptsPerIp", DefaultMaxLoginAttemptsPerIpPerMinute);
            var windowMinutes = GetConfigInt("FaceAi:RateLimit:WindowMinutes", DefaultRateLimitWindowMinutes);
            var window = TimeSpan.FromMinutes(windowMinutes);

            var currentCount = await _logRepository.GetLoginAttemptCountByIpAsync(
                ipAddress, window, cancellationToken);

            if (currentCount >= maxAttempts)
            {
                _logger.LogWarning(
                    "IP rate limit exceeded: {IpAddress} has {Count}/{Max} attempts in {Window}min",
                    ipAddress, currentCount, maxAttempts, windowMinutes);

                return new RateLimitResult
                {
                    IsLimited = true,
                    CurrentCount = currentCount,
                    MaxAllowed = maxAttempts,
                    RetryAfter = window,
                    Message = $"Too many login attempts from this IP. Please try again in {windowMinutes} minute(s)."
                };
            }

            return new RateLimitResult
            {
                IsLimited = false,
                CurrentCount = currentCount,
                MaxAllowed = maxAttempts
            };
        }

        public async Task<RateLimitResult> CheckUserRateLimitAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new RateLimitResult { IsLimited = false };
            }

            var maxAttempts = GetConfigInt("FaceAi:RateLimit:MaxLoginAttemptsPerUser", DefaultMaxLoginAttemptsPerUserPerMinute);
            var windowMinutes = GetConfigInt("FaceAi:RateLimit:WindowMinutes", DefaultRateLimitWindowMinutes);
            var window = TimeSpan.FromMinutes(windowMinutes);

            var currentCount = await _logRepository.GetLoginAttemptCountByUserAsync(
                userId, window, cancellationToken);

            if (currentCount >= maxAttempts)
            {
                _logger.LogWarning(
                    "User rate limit exceeded: {UserId} has {Count}/{Max} attempts in {Window}min",
                    userId, currentCount, maxAttempts, windowMinutes);

                return new RateLimitResult
                {
                    IsLimited = true,
                    CurrentCount = currentCount,
                    MaxAllowed = maxAttempts,
                    RetryAfter = window,
                    Message = $"Too many login attempts for this account. Please try again in {windowMinutes} minute(s)."
                };
            }

            return new RateLimitResult
            {
                IsLimited = false,
                CurrentCount = currentCount,
                MaxAllowed = maxAttempts
            };
        }

        private int GetConfigInt(string key, int defaultValue)
        {
            var value = _configuration[key];
            return int.TryParse(value, out var result) ? result : defaultValue;
        }

        private static string? TruncateUserAgent(string? userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return null;

            return userAgent.Length > 500
                ? userAgent[..500]
                : userAgent;
        }
    }
}
