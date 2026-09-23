using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.Workflow
{
    public static class StudentAnnouncementWorkflowMapping
    {
        public static StudentAnnouncementWorkflowItemDto ToWorkflowItem(
            Application.Entity.Announcement announcement,
            AnnouncementStudent? link,
            DateTime utcNow,
            bool includeContent)
        {
            return new StudentAnnouncementWorkflowItemDto
            {
                AnnouncementId = announcement.Id,
                Title = announcement.Title,
                Description = announcement.Description,
                Type = announcement.Type?.ToUpperInvariant() ?? AnnouncementConstants.Type.Normal,
                St = AnnouncementLifecycle.ComputeStatus(announcement.PublishDate, announcement.ExpiredDate, utcNow),
                ScopeType = announcement.ScopeType?.ToUpperInvariant() ?? AnnouncementConstants.Scope.Public,
                ScopeValue = announcement.ScopeValue,
                PublishDate = AdminAnnouncementMapping.AsUtc(announcement.PublishDate),
                ExpiredDate = AdminAnnouncementMapping.AsUtc(announcement.ExpiredDate),
                Content = includeContent ? announcement.Content : null,
                ViewedAt = link?.ViewedAt,
                AcknowledgedAt = link?.AcknowledgedAt
            };
        }
    }
}
