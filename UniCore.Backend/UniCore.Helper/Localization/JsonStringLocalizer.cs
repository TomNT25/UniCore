using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace UniCore.Helper.Localization
{
    public class JsonStringLocalizer : IJsonStringLocalizer
    {
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> _cultureCache = new();
        private readonly string _resourcesPath;

        public JsonStringLocalizer()
        {
            var baseDir = AppContext.BaseDirectory;
            _resourcesPath = Path.Combine(baseDir, "Resources");
        }

        public LocalizedString this[string key]
        {
            get
            {
                var value = GetString(key);
                return new LocalizedString(key, value, resourceNotFound: value == key);
            }
        }

        public LocalizedString this[string key, params object[] arguments]
        {
            get
            {
                var value = GetString(key, arguments);
                return new LocalizedString(key, value, resourceNotFound: false);
            }
        }

        public string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            var culture = CultureInfo.CurrentUICulture.Name;
            var dictionary = GetOrLoadDictionary(culture);

            if (dictionary.TryGetValue(key, out var localizedValue))
            {
                return localizedValue;
            }

            // Fallback to default culture (en-US) if current culture does not match
            if (!culture.Equals("en-US", StringComparison.OrdinalIgnoreCase))
            {
                var defaultDictionary = GetOrLoadDictionary("en-US");
                if (defaultDictionary.TryGetValue(key, out var fallbackValue))
                {
                    return fallbackValue;
                }
            }

            return key;
        }

        public string GetString(string key, params object[] arguments)
        {
            var format = GetString(key);
            try
            {
                return string.Format(CultureInfo.CurrentCulture, format, arguments);
            }
            catch
            {
                return format;
            }
        }

        private Dictionary<string, string> GetOrLoadDictionary(string cultureName)
        {
            return _cultureCache.GetOrAdd(cultureName, culture =>
            {
                var filePath = Path.Combine(_resourcesPath, $"{culture}.json");
                if (!File.Exists(filePath))
                {
                    // Fallback search in working directory / Resources
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", $"{culture}.json");
                }

                if (File.Exists(filePath))
                {
                    try
                    {
                        var json = File.ReadAllText(filePath);
                        var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                        if (parsed != null)
                        {
                            return new Dictionary<string, string>(parsed, StringComparer.OrdinalIgnoreCase);
                        }
                    }
                    catch
                    {
                        // Ignore read/parse errors
                    }
                }

                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            });
        }
    }
}
