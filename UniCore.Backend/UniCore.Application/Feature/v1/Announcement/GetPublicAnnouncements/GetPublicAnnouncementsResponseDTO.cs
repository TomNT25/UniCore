using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;

namespace UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements
{
    public class GetPublicAnnouncementsResponseDTO
    {
        public List<AdminAnnouncementItemDTO> Items { get; set; } = new();
    }
}
