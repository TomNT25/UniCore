using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<List<string>> GetActiveStudentIdsAsync(CancellationToken cancellationToken = default);
        Task<List<string>> GetActiveVerifiedStudentIdsAsync(CancellationToken cancellationToken = default);
        Task<List<string>> GetActiveStudentIdsByIdsAsync(IEnumerable<string> studentIds, CancellationToken cancellationToken = default);
        Task<List<(string StudentId, string Email)>> GetActiveStudentEmailsByIdsAsync(
            IEnumerable<string> studentIds,
            CancellationToken cancellationToken = default);

        Task<List<string>> GetActiveVerifiedStudentIdsByIdsAsync(
            IEnumerable<string> studentIds,
            CancellationToken cancellationToken = default);

        Task<(List<User> Items, int TotalCount)> SearchActiveVerifiedStudentsAsync(
            string? search,
            int limit,
            CancellationToken cancellationToken = default);
    }
}
