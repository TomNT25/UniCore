using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class StudentClassRepository : RepositoryEFCoreBase<StudentClass>, IStudentClassRepository
    {
        public StudentClassRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<string?> GetStudentClassIdAsync(
            string classId, 
            string studentId, 
            CancellationToken ct)
        {
            var result = await _dbSet.Where(x => x.ClassId.Equals(classId) && x.StudentId.Equals(studentId))
                        .Select(x => x.Id)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ct);
            return result;
        }

        public async Task<IEnumerable<string>?> GetUserIdsByStudentClassIdAsync(
            string classId, 
            string studentId,
            CancellationToken ct)
        {
            var results = await _dbSet
                                .Where(x => x.ClassId.Equals(classId) && !x.StudentId.Equals(studentId))
                                .Select(x => x.StudentId)
                                .Take(30)
                                .AsNoTracking()
                                .ToListAsync(ct);
            
            return results;
        }

        public async Task<IEnumerable<string>?> GetClassIdsAsync(string studentId, CancellationToken ct)
        {
            var results = await _dbSet
                                .Where(x => x.StudentId.Equals(studentId))
                                .Select(x => x.ClassId)
                                .Take(30)
                                .AsNoTracking()
                                .ToListAsync(ct);
            return results;
        }

    }
}
