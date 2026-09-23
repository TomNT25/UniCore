using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IStudentClassRepository : IRepository<StudentClass>
    {
        Task<string?> GetStudentClassIdAsync(string classId, string studentId, CancellationToken ct);
        Task<IEnumerable<string>?> GetClassIdsAsync(string studentId, CancellationToken ct);
        Task<IEnumerable<string>?> GetUserIdsByStudentClassIdAsync(string classId, string studentId, CancellationToken ct);
        Task<List<string>> GetActiveStudentIdsByClassIdAsync(string classId, CancellationToken cancellationToken = default);
        Task<List<string>> GetActiveStudentIdsByDepartmentIdAsync(string departmentId, CancellationToken cancellationToken = default);
    }
}
