namespace UniCore.Application.Feature.v1.Announcement.PreviewRecipients
{
    public class PreviewRecipientsResponseDTO
    {
        /// <summary>Null when scope is PUBLIC (unlimited / no snapshot).</summary>
        public int? RecipientCount { get; set; }
        public bool IsPublicUnlimited { get; set; }
        public List<string> StudentIds { get; set; } = new();
    }
}
