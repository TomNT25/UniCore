using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    public static class AnnouncementTargetLimit
    {
        public static int Normalize(int? limit)
        {
            if (!limit.HasValue || limit.Value <= 0)
            {
                return AnnouncementTargetConstants.DefaultLimit;
            }

            return Math.Min(limit.Value, AnnouncementTargetConstants.MaxLimit);
        }
    }
}
