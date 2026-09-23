using Microsoft.Extensions.Localization;

namespace UniCore.Helper.Localization
{
    public interface IJsonStringLocalizer
    {
        LocalizedString this[string key] { get; }
        LocalizedString this[string key, params object[] arguments] { get; }
        string GetString(string key);
        string GetString(string key, params object[] arguments);
    }
}
