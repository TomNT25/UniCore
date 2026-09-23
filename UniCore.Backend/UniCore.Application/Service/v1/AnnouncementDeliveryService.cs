using Microsoft.Extensions.Options;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;
using UniCore.Helper.Options;

namespace UniCore.Application.Service.v1
{
    public class AnnouncementDeliveryService : IAnnouncementDeliveryService
    {
        private readonly IAnnouncementAudienceService _audienceService;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IAnnouncementEmailLogRepository _emailLogRepository;
        private readonly IAnnouncementEmailWhitelistRepository _whitelistRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly EmailOptions _emailOptions;

        public AnnouncementDeliveryService(
            IAnnouncementAudienceService audienceService,
            IAnnouncementStudentRepository announcementStudentRepository,
            IAnnouncementEmailLogRepository emailLogRepository,
            IAnnouncementEmailWhitelistRepository whitelistRepository,
            IUserRepository userRepository,
            IEmailSender emailSender,
            IOptions<EmailOptions> emailOptions)
        {
            _audienceService = audienceService;
            _announcementStudentRepository = announcementStudentRepository;
            _emailLogRepository = emailLogRepository;
            _whitelistRepository = whitelistRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _emailOptions = emailOptions.Value;
        }

        public async Task ApplyAudienceSideEffectsAsync(
            Announcement announcement,
            IEnumerable<string>? targetStudentIds,
            CancellationToken cancellationToken = default)
        {
            var scopeType = announcement.ScopeType.Trim().ToUpperInvariant();
            var type = announcement.Type.Trim().ToUpperInvariant();
            var requiresSideEffects = AnnouncementConstants.Type.RequiresSideEffects(type);
            var isSpecific = scopeType == AnnouncementConstants.Scope.SpecificStudents;

            if (scopeType == AnnouncementConstants.Scope.Public)
            {
                await _announcementStudentRepository.ReplaceRecipientsAsync(
                    announcement.Id, Array.Empty<string>(), cancellationToken);
                announcement.RecipientCount = null;
                return;
            }

            if (!requiresSideEffects && !isSpecific)
            {
                await _announcementStudentRepository.ReplaceRecipientsAsync(
                    announcement.Id, Array.Empty<string>(), cancellationToken);
                announcement.RecipientCount = null;
                return;
            }

            var studentIds = await _audienceService.ResolveStudentIdsAsync(
                scopeType,
                announcement.ScopeValue,
                targetStudentIds,
                cancellationToken);

            await _announcementStudentRepository.ReplaceRecipientsAsync(
                announcement.Id, studentIds, cancellationToken);
            announcement.RecipientCount = studentIds.Count;

            if (!requiresSideEffects || studentIds.Count == 0 || !_emailOptions.Enabled)
            {
                return;
            }

            await SendEmailsAsync(announcement, studentIds, cancellationToken);
        }

        private async Task SendEmailsAsync(
            Announcement announcement,
            IReadOnlyList<string> studentIds,
            CancellationToken cancellationToken)
        {
            var whitelist = await _whitelistRepository.GetActiveEntriesAsync(cancellationToken);
            if (whitelist.Count == 0)
            {
                return;
            }

            var emails = AnnouncementLifecycle.NormalizeIds(
                whitelist.Where(w => w.Type == AnnouncementConstants.Whitelist.Email).Select(w => w.Value.ToLowerInvariant()));
            var domains = AnnouncementLifecycle.NormalizeIds(
                whitelist.Where(w => w.Type == AnnouncementConstants.Whitelist.Domain).Select(w => w.Value.ToLowerInvariant()));

            var recipients = await _userRepository.GetActiveStudentEmailsByIdsAsync(studentIds, cancellationToken);
            var allowed = recipients
                .Where(r => IsWhitelisted(r.Email, emails, domains))
                .ToList();

            if (allowed.Count == 0)
            {
                return;
            }

            var subject = $"[{announcement.Type}] {announcement.Title}";
            var body = BuildEmailBody(announcement);
            var now = DateTime.UtcNow;
            var logs = new List<AnnouncementEmailLog>();

            foreach (var (studentId, email) in allowed)
            {
                var log = new AnnouncementEmailLog
                {
                    Id = Guid.NewGuid().ToString(),
                    AnnouncementId = announcement.Id,
                    StudentId = studentId,
                    Email = email,
                    Status = AnnouncementConstants.EmailLogStatus.Pending,
                    CreatedAt = now
                };

                try
                {
                    await _emailSender.SendAsync(email, subject, body, cancellationToken);
                    log.Status = AnnouncementConstants.EmailLogStatus.Sent;
                    log.SentAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    log.Status = AnnouncementConstants.EmailLogStatus.Failed;
                    log.ErrorMessage = ex.Message.Length > 500 ? ex.Message[..500] : ex.Message;
                }

                logs.Add(log);
            }

            await _emailLogRepository.AddRangeLogsAsync(logs, cancellationToken);
        }

        private static bool IsWhitelisted(string email, List<string> emails, List<string> domains)
        {
            var normalized = email.Trim().ToLowerInvariant();
            if (emails.Contains(normalized, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            var at = normalized.LastIndexOf('@');
            if (at < 0 || at == normalized.Length - 1)
            {
                return false;
            }

            var domain = normalized[(at + 1)..];
            return domains.Contains(domain, StringComparer.OrdinalIgnoreCase);
        }

        private static string BuildEmailBody(Announcement announcement)
        {
            var description = string.IsNullOrWhiteSpace(announcement.Description)
                ? string.Empty
                : $"<p>{System.Net.WebUtility.HtmlEncode(announcement.Description)}</p>";

            return $"""
                <h2>{System.Net.WebUtility.HtmlEncode(announcement.Title)}</h2>
                {description}
                <div>{announcement.Content}</div>
                <p><small>Publish: {announcement.PublishDate:u} — Expire: {announcement.ExpiredDate:u}</small></p>
                """;
        }
    }
}
