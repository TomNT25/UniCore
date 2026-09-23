using System.Text.Json;

namespace UniCore.Application.Feature.v1.Announcement
{
    /// <summary>
    /// Stores one or many scope target IDs in announcements.scope_value (JSON array or single id).
    /// </summary>
    public static class AnnouncementScopeStorage
    {
        public const int MaxStoredLength = 4000;

        public static List<string> ParseTargetIds(string? scopeValue)
        {
            if (string.IsNullOrWhiteSpace(scopeValue))
            {
                return new List<string>();
            }

            var trimmed = scopeValue.Trim();
            if (trimmed.StartsWith('['))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<List<string>>(trimmed);
                    return AnnouncementLifecycle.NormalizeIds(parsed);
                }
                catch (JsonException)
                {
                    return new List<string> { trimmed };
                }
            }

            if (trimmed.Contains(','))
            {
                return AnnouncementLifecycle.NormalizeIds(trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries));
            }

            return new List<string> { trimmed };
        }

        public static string? SerializeTargetIds(IEnumerable<string>? ids)
        {
            var list = AnnouncementLifecycle.NormalizeIds(ids);
            if (list.Count == 0)
            {
                return null;
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            return JsonSerializer.Serialize(list);
        }

        public static string? BuildScopeValueForRequest(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targets)
        {
            var scope = scopeType.Trim().ToUpperInvariant();
            if (scope is UniCore.Helper.Constant.AnnouncementConstants.Scope.Public
                or UniCore.Helper.Constant.AnnouncementConstants.Scope.Students
                or UniCore.Helper.Constant.AnnouncementConstants.Scope.SpecificStudents)
            {
                return null;
            }

            var fromTargets = AnnouncementLifecycle.NormalizeIds(targets);
            if (fromTargets.Count > 0)
            {
                return SerializeTargetIds(fromTargets);
            }

            return string.IsNullOrWhiteSpace(scopeValue) ? null : scopeValue.Trim();
        }

        public static void EnsureStoredLength(string? serialized)
        {
            if (serialized != null && serialized.Length > MaxStoredLength)
            {
                throw new InvalidOperationException(
                    $"Scope targets exceed maximum storage length ({MaxStoredLength} characters).");
            }
        }
    }
}
