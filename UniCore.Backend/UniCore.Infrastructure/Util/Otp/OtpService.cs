using System.Security.Cryptography;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Util;

namespace UniCore.Infrastructure.Util.Otp
{
    public class OtpService : IOtpService
    {
        private readonly ICacheService _cacheService;

        public OtpService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public string GenerateOtp(string key, TimeSpan? expiration = null)
        {
            return GenerateOtpAsync(key, expiration).GetAwaiter().GetResult();
        }

        public bool VerifyOtp(string key, string otp)
        {
            return VerifyOtpAsync(key, otp).GetAwaiter().GetResult();
        }

        public void InvalidateOtp(string key)
        {
            InvalidateOtpAsync(key).GetAwaiter().GetResult();
        }

        public async Task<string> GenerateOtpAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var otpNumber = RandomNumberGenerator.GetInt32(100000, 1000000);
            var otpCode = otpNumber.ToString();
            var expiry = expiration ?? TimeSpan.FromMinutes(15);

            var cacheKey = GetCacheKey(key);
            await _cacheService.SetStringAsync(cacheKey, otpCode, expiry, cancellationToken);

            return otpCode;
        }

        public async Task<bool> VerifyOtpAsync(string key, string otp, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(otp))
                return false;

            var cacheKey = GetCacheKey(key);
            var cachedOtp = await _cacheService.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedOtp))
            {
                if (string.Equals(cachedOtp.Trim(), otp.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    await _cacheService.RemoveAsync(cacheKey, cancellationToken);
                    return true;
                }
            }

            return false;
        }

        public async Task InvalidateOtpAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cacheService.RemoveAsync(GetCacheKey(key), cancellationToken);
        }

        private static string GetCacheKey(string key) => $"OTP_{key.Trim().ToLowerInvariant()}";
    }
}
