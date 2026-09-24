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
                Data = data,
                Meta = new AnnouncementPaginationMetaDTO
                {
                    Page = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalItems = pagedResult.TotalRecords,
                    TotalPages = pagedResult.TotalPages,
                    HasNextPage = pagedResult.HasNextPage,
                    HasPreviousPage = pagedResult.HasPreviousPage
                }
            };
        }

        private static Expression<Func<UniCore.Application.Entity.Announcement, bool>>? BuildFilter(GetAllAnnouncementRequestDTO request)
        {
            var hasSearch = !string.IsNullOrWhiteSpace(request.SearchTerm);
            var hasStatus = !string.IsNullOrWhiteSpace(request.Status);
            var now = DateTime.UtcNow;
            var status = hasStatus ? request.Status!.Trim().ToUpperInvariant() : null;

            if (!hasSearch && !hasStatus)
            {
                return null;
            }

            if (hasSearch && status == AnnouncementConstants.Status.Upcoming)
            {
                var term = request.SearchTerm!;
                return a =>
                    ((a.Title != null && a.Title.Contains(term)) || (a.Code != null && a.Code.Contains(term)))
                    && now < a.PublishDate;
            }

            if (hasSearch && status == AnnouncementConstants.Status.Active)
            {
                var term = request.SearchTerm!;
                return a =>
                    ((a.Title != null && a.Title.Contains(term)) || (a.Code != null && a.Code.Contains(term)))
                    && now >= a.PublishDate && now < a.ExpiredDate;
            }

            if (hasSearch && status == AnnouncementConstants.Status.Expired)
            {
                var term = request.SearchTerm!;
                return a =>
                    ((a.Title != null && a.Title.Contains(term)) || (a.Code != null && a.Code.Contains(term)))
                    && now >= a.ExpiredDate;
            }

            if (hasSearch)
            {
                var term = request.SearchTerm!;
                return a => (a.Title != null && a.Title.Contains(term)) || (a.Code != null && a.Code.Contains(term));
            }

            if (status == AnnouncementConstants.Status.Upcoming)
            {
                return a => now < a.PublishDate;
            }

            if (status == AnnouncementConstants.Status.Active)
            {
                return a => now >= a.PublishDate && now < a.ExpiredDate;
            }

            if (status == AnnouncementConstants.Status.Expired)
            {
                return a => now >= a.ExpiredDate;
            }

            return null;
        }
    }
}
