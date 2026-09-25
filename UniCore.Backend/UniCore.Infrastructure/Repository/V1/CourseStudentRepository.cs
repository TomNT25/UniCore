using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.DTO;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static UniCore.Helper.Constant.MessageConstants;

namespace UniCore.Infrastructure.Repository.V1
{
    public class CourseStudentRepository : RepositoryEFCoreBase<CourseStudent>, ICourseStudentRepository
    {
        public CourseStudentRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<CourseStudent>?> GetByStuIdCourseIdsAsync(string studentId, CancellationToken ct)
        {
            var result = await _dbSet
                            .Where(x => x.UserId.Equals(studentId))
                            .Take(30)
                            .AsNoTracking()
                            .ToListAsync(ct);

            return result;
        }

        public async Task<IEnumerable<string>?> GetCourseIdsByStuIdAsync(string stuId, CancellationToken ct)
        {
            var results = await _dbSet
                .Where(x => x.UserId.Equals(stuId))
                .Select(x => x.CourseId)
                .Take(30)
                .AsNoTracking()
                .ToListAsync(ct);

            return results;
        }

        public async Task<PageNumberPaginationResponse<CourseStudent>> GetPaginatedByStuIdCourseIdsAsync(string studentId,
    PageNumberPaginationRequest request,
    CancellationToken cancellationToken = default,
    Expression<Func<CourseStudent, bool>>? filter = null)
        {
            var query = _dbSet.AsNoTracking().Where(sc => sc.UserId == studentId);

            if(filter is not null)
            {
                query.Where(filter);
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
            var skip = (pageNumber - 1) * pageSize;

            var items = await query
                            .Include(sc => sc.Course)
                                .ThenInclude(c => c.Department)
                            .Include(sc => sc.Schedules)
                            .Skip(skip)
                            .Take(pageSize)
                            .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var results = new PageNumberPaginationResponse<CourseStudent>()
            {
                Items = items,
                Metadata = new PageNumberPaginationMetaResponse
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    HasNextPage = pageNumber < totalPages,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return results;
        }

        public async Task<CourseStudent?> GetDetailedStuIdCourseIdsAsync(string studentId, 
            string courseId,
            CancellationToken cancellationToken)
        {

            var item = await _dbSet.AsNoTracking()
                            .Where(sc => sc.UserId == studentId && sc.CourseId.Equals(courseId))
                            .Include(sc => sc.Course)
                                .ThenInclude(c => c.Department)
                            .Include(sc => sc.Schedules)
                            .FirstOrDefaultAsync(cancellationToken);
            return item;
        }

        public async Task<List<string>> GetActiveStudentIdsByCourseIdAsync(string courseId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(cs =>
                    cs.CourseId == courseId &&
                    cs.IsActive &&
                    !cs.IsDeleted)
                .Select(cs => cs.User.StudentCode)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
