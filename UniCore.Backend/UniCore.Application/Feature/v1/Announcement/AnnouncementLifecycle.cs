using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement
{
    public static class AnnouncementLifecycle
    {
        public static string ComputeStatus(DateTime publishDate, DateTime expiredDate, DateTime? utcNow = null)
        {
            var now = utcNow ?? DateTime.UtcNow;

            if (now < publishDate)
            {
                return AnnouncementConstants.Status.Upcoming;
            }

            if (now >= expiredDate)
            {
                return AnnouncementConstants.Status.Expired;
            }

            return AnnouncementConstants.Status.Active;
        }

        public static string? NormalizeScopeValue(string scopeType, string? scopeValue)
        {
            var scope = scopeType.Trim().ToUpperInvariant();
            if (scope is AnnouncementConstants.Scope.Public
                or AnnouncementConstants.Scope.Students
                or AnnouncementConstants.Scope.SpecificStudents)
            {
                return null;
            }

            return string.IsNullOrWhiteSpace(scopeValue) ? null : scopeValue.Trim();
        }

        public static List<string> NormalizeIds(IEnumerable<string>? ids)
        {
            if (ids == null)
            {
                return new List<string>();
            }

            return ids
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
