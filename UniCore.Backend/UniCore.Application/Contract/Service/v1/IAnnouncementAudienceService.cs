namespace UniCore.Application.Contract.Service.v1
{
    public interface IAnnouncementAudienceService
    {
        Task ValidateScopeAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<string>> ResolveStudentIdsAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default);
    }
}
