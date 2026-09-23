using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement
{
    public static class AdminAnnouncementMapping
    {
        public static AdminAnnouncementItemDTO ToAdminItem(
            Application.Entity.Announcement entity,
            DateTime utcNow,
            bool includeContent)
        {
            return new AdminAnnouncementItemDTO
            {
                Id = entity.Id,
                Code = entity.Code ?? string.Empty,
                Title = entity.Title,
                Description = entity.Description ?? string.Empty,
                Content = includeContent ? entity.Content : null,
                Type = NormalizeType(entity.Type),
                PublishDate = AsUtc(entity.PublishDate),
                ExpiredDate = AsUtc(entity.ExpiredDate),
                RecipientCount = ResolveRecipientCount(entity.ScopeType, entity.RecipientCount),
                ScopeType = entity.ScopeType?.ToUpperInvariant() ?? AnnouncementConstants.Scope.Public,
                Status = AnnouncementLifecycle.ComputeStatus(entity.PublishDate, entity.ExpiredDate, utcNow)
            };
        }

        public static AdminAnnouncementItemDTO ToAdminItem(
            AnnouncementDTO dto,
            DateTime utcNow,
            bool includeContent)
        {
            return new AdminAnnouncementItemDTO
            {
                Id = dto.Id,
                Code = dto.Code ?? string.Empty,
                Title = dto.Title,
                Description = dto.Description ?? string.Empty,
                Content = includeContent ? dto.Content : null,
                Type = NormalizeType(dto.Type),
                PublishDate = AsUtc(dto.PublishDate),
                ExpiredDate = AsUtc(dto.ExpiredDate),
                RecipientCount = ResolveRecipientCount(dto.ScopeType, dto.RecipientCount),
                ScopeType = dto.ScopeType?.ToUpperInvariant() ?? AnnouncementConstants.Scope.Public,
                Status = AnnouncementLifecycle.ComputeStatus(dto.PublishDate, dto.ExpiredDate, utcNow)
            };
        }

        private static int? ResolveRecipientCount(string? scopeType, int? recipientCount)
        {
            if (string.Equals(scopeType, AnnouncementConstants.Scope.Public, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return recipientCount;
        }

        private static string NormalizeType(string? type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return AnnouncementConstants.Type.Normal;
            }

            return type.Trim().ToUpperInvariant();
        }

        public static DateTime AsUtc(DateTime value)
        {
            return value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
            };
        }
    }
}
