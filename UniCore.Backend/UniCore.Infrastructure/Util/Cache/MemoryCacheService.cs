using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Cache;

namespace UniCore.Infrastructure.Util.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<MemoryCacheService> _logger;

        public MemoryCacheService(
            IMemoryCache memoryCache,
            ILogger<MemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default;

            var cacheKey = FormatKey(key);

            if (_memoryCache.TryGetValue(cacheKey, out T? value))
            {
                return value;
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
                return;

            var cacheKey = FormatKey(key);
            var expiry = expiration ?? TimeSpan.FromMinutes(15);

            _memoryCache.Set(cacheKey, value, expiry);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            var cacheKey = FormatKey(key);

            _memoryCache.Remove(cacheKey);
        }

        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            var result = await GetStringAsync(key, cancellationToken);
            return result != null;
        }

        public async Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var cacheKey = FormatKey(key);

            if (_memoryCache.TryGetValue(cacheKey, out string? value))
            {
                return value;
            }

            return null;
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            var cacheKey = FormatKey(key);
            var expiry = expiration ?? TimeSpan.FromMinutes(15);

            _memoryCache.Set(cacheKey, value, expiry);
        }

        private static string FormatKey(string key) => key.Trim();
    }
}
