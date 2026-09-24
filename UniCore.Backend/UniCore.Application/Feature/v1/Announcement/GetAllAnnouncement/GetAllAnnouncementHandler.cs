using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    public class GetAllAnnouncementHandler : IRequestHandler<GetAllAnnouncementRequestDTO, GetAllAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public GetAllAnnouncementHandler(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<GetAllAnnouncementResponseDTO> HandleAsync(GetAllAnnouncementRequestDTO request, CancellationToken cancellationToken)
        {
            ApplySorting(request);
            Expression<Func<UniCore.Application.Entity.Announcement, bool>>? filter = BuildFilter(request);

            var pagedResult = await _announcementRepository.GetPageNumberPaginationAsync<AnnouncementDTO>(
                request,
                filter,
                cancellationToken);

            var utcNow = DateTime.UtcNow;
            var data = pagedResult.Items
                .Select(item => AdminAnnouncementMapping.ToAdminItem(item, utcNow, includeContent: false))
                .ToList();

            return new GetAllAnnouncementResponseDTO
            {
                Items = data,
                Metadata = pagedResult.Metadata
            };
        }

        private static void ApplySorting(GetAllAnnouncementRequestDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                request.SortColumn = MapSortBy(request.SortBy);
            }
            else if (string.IsNullOrWhiteSpace(request.SortColumn))
            {
                request.SortColumn = nameof(UniCore.Application.Entity.Announcement.PublishDate);
                if (string.IsNullOrWhiteSpace(request.SortOrder))
                {
                    request.SortDescending = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(request.SortOrder))
            {
                request.SortDescending = string.Equals(
                    request.SortOrder.Trim(),
                    "desc",
                    StringComparison.OrdinalIgnoreCase);
            }
        }

        private static string MapSortBy(string sortBy)
        {
            return sortBy.Trim().ToLowerInvariant() switch
            {
"startdate" or "publishdate" or "publish_date" =>
                    nameof(UniCore.Application.Entity.Announcement.PublishDate),
                "enddate" or "expireddate" or "expired_date" =>
                    nameof(UniCore.Application.Entity.Announcement.ExpiredDate),
                "title" => nameof(UniCore.Application.Entity.Announcement.Title),
                "code" => nameof(UniCore.Application.Entity.Announcement.Code),
                "type" or "announcementtype" => nameof(UniCore.Application.Entity.Announcement.Type),
                "scopetype" or "scope_type" => nameof(UniCore.Application.Entity.Announcement.ScopeType),
                "status" => nameof(UniCore.Application.Entity.Announcement.Status),
                "createdat" or "created_at" => nameof(UniCore.Application.Entity.Announcement.CreatedAt),
                "updatedat" or "updated_at" => nameof(UniCore.Application.Entity.Announcement.UpdatedAt),
                _ => sortBy.Trim()
            };
        }

        private static Expression<Func<UniCore.Application.Entity.Announcement, bool>>? BuildFilter(GetAllAnnouncementRequestDTO request)
        {
            var hasSearch = !string.IsNullOrWhiteSpace(request.SearchTerm);
            var status = !string.IsNullOrWhiteSpace(request.Status)
                ? request.Status.Trim().ToUpperInvariant()
                : null;
            var type = !string.IsNullOrWhiteSpace(request.AnnouncementType)
                ? request.AnnouncementType.Trim().ToUpperInvariant()
                : null;
            var scopeType = !string.IsNullOrWhiteSpace(request.ScopeType)
                ? request.ScopeType.Trim().ToUpperInvariant()
                : null;
            var activeFrom = request.ActiveFrom;
            var activeTo = request.ActiveTo;

            var applyStatus = status is AnnouncementConstants.Status.Upcoming
                or AnnouncementConstants.Status.Active
                or AnnouncementConstants.Status.Expired;
            var hasType = type != null;
            var hasScope = scopeType != null;
            var hasActiveFrom = activeFrom.HasValue;
            var hasActiveTo = activeTo.HasValue;

            if (!hasSearch && !applyStatus && !hasType && !hasScope && !hasActiveFrom && !hasActiveTo)
            {
                return null;
            }

            var term = hasSearch ? request.SearchTerm!.Trim() : null;
            var now = DateTime.UtcNow;

            return a =>
                (!hasSearch
                    || (a.Title != null && a.Title.Contains(term!))
                    || (a.Code != null && a.Code.Contains(term!)))
                && (!hasType || a.Type == type)
                && (!hasScope || a.ScopeType == scopeType)
                && (!applyStatus
                    || (status == AnnouncementConstants.Status.Upcoming && now < a.PublishDate)
                    || (status == AnnouncementConstants.Status.Active && now >= a.PublishDate && now < a.ExpiredDate)
                    || (status == AnnouncementConstants.Status.Expired && now >= a.ExpiredDate))
                && (!hasActiveFrom || a.ExpiredDate > activeFrom!.Value)
                && (!hasActiveTo || a.PublishDate <= activeTo!.Value);
        }
    }
}