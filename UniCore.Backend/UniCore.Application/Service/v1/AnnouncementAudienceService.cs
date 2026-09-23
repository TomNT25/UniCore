using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Announcement;
using UniCore.Helper.Constant;

namespace UniCore.Application.Service.v1
{
    public class AnnouncementAudienceService : IAnnouncementAudienceService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;

        public AnnouncementAudienceService(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            ISchoolClassRepository schoolClassRepository,
            IStudentClassRepository studentClassRepository,
            ICourseRepository courseRepository,
            ICourseStudentRepository courseStudentRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _schoolClassRepository = schoolClassRepository;
            _studentClassRepository = studentClassRepository;
            _courseRepository = courseRepository;
            _courseStudentRepository = courseStudentRepository;
        }

        public async Task ValidateScopeAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default)
        {
            var scope = scopeType.Trim().ToUpperInvariant();
            if (!AnnouncementConstants.Scope.Supported.Contains(scope))
            {
                throw new InvalidOperationException(
                    "ScopeType must be PUBLIC, STUDENTS, DEPARTMENT, CLASS, COURSE, or SPECIFIC_STUDENTS.");
            }

            var studentTargets = AnnouncementLifecycle.NormalizeIds(targetStudentIds);

            switch (scope)
            {
                case AnnouncementConstants.Scope.Public:
                    if (!string.IsNullOrWhiteSpace(scopeValue) || studentTargets.Count > 0)
                    {
                        throw new InvalidOperationException("PUBLIC scope must not include scopeValue or targetStudentIds.");
                    }
                    break;

                case AnnouncementConstants.Scope.Students:
                    if (!string.IsNullOrWhiteSpace(scopeValue) || studentTargets.Count > 0)
                    {
                        throw new InvalidOperationException("STUDENTS scope must not include scopeValue or targetStudentIds.");
                    }
                    break;

                case AnnouncementConstants.Scope.Department:
                    await EnsureScopeTargetsAsync(scope, scopeValue, studentTargets, EnsureDepartmentIdsAsync, cancellationToken);
                    break;

                case AnnouncementConstants.Scope.Class:
                    await EnsureScopeTargetsAsync(scope, scopeValue, studentTargets, EnsureClassIdsAsync, cancellationToken);
                    break;

                case AnnouncementConstants.Scope.Course:
                    await EnsureScopeTargetsAsync(scope, scopeValue, studentTargets, EnsureCourseIdsAsync, cancellationToken);
                    break;

                case AnnouncementConstants.Scope.SpecificStudents:
                    if (!string.IsNullOrWhiteSpace(scopeValue))
                    {
                        throw new InvalidOperationException("SPECIFIC_STUDENTS scopeValue must be null; provide targetStudentIds.");
                    }
                    if (studentTargets.Count == 0)
                    {
                        throw new InvalidOperationException("SPECIFIC_STUDENTS requires at least one targetStudentId.");
                    }
                    var active = await _userRepository.GetActiveVerifiedStudentIdsByIdsAsync(studentTargets, cancellationToken);
                    if (active.Count != studentTargets.Count)
                    {
                        var missing = studentTargets.Except(active, StringComparer.OrdinalIgnoreCase);
                        throw new KeyNotFoundException($"Active verified student(s) not found: {string.Join(", ", missing)}");
                    }
                    break;
            }
        }

        public async Task<IReadOnlyList<string>> ResolveStudentIdsAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default)
        {
            await ValidateScopeAsync(scopeType, scopeValue, targetStudentIds, cancellationToken);
            var scope = scopeType.Trim().ToUpperInvariant();

            return scope switch
            {
                AnnouncementConstants.Scope.Public => Array.Empty<string>(),
                AnnouncementConstants.Scope.Students =>
                    await _userRepository.GetActiveVerifiedStudentIdsAsync(cancellationToken),
                AnnouncementConstants.Scope.Department =>
                    await ResolveStudentsByDepartmentIdsAsync(scopeValue, cancellationToken),
                AnnouncementConstants.Scope.Class =>
                    await ResolveStudentsByClassIdsAsync(scopeValue, cancellationToken),
                AnnouncementConstants.Scope.Course =>
                    await ResolveStudentsByCourseIdsAsync(scopeValue, cancellationToken),
                AnnouncementConstants.Scope.SpecificStudents =>
                    await _userRepository.GetActiveVerifiedStudentIdsByIdsAsync(
                        AnnouncementLifecycle.NormalizeIds(targetStudentIds), cancellationToken),
                _ => Array.Empty<string>()
            };
        }

        private static async Task EnsureScopeTargetsAsync(
            string scopeLabel,
            string? scopeValue,
            List<string> studentTargets,
            Func<List<string>, CancellationToken, Task> ensureEntitiesAsync,
            CancellationToken cancellationToken)
        {
            if (studentTargets.Count > 0)
            {
                throw new InvalidOperationException($"{scopeLabel} must not include targetStudentIds.");
            }

            var ids = AnnouncementScopeStorage.ParseTargetIds(scopeValue);
            if (ids.Count == 0)
            {
                throw new InvalidOperationException($"{scopeLabel} requires at least one target id in scope_value.");
            }

            await ensureEntitiesAsync(ids, cancellationToken);
        }

        private async Task EnsureDepartmentIdsAsync(List<string> ids, CancellationToken ct)
        {
            foreach (var id in ids)
            {
                var dept = await _departmentRepository.GetByIdAsync(id, ct);
                if (dept == null || dept.IsDeleted || !dept.IsActive)
                {
                    throw new KeyNotFoundException($"Active department '{id}' was not found.");
                }
            }
        }

        private async Task EnsureClassIdsAsync(List<string> ids, CancellationToken ct)
        {
            foreach (var id in ids)
            {
                var schoolClass = await _schoolClassRepository.GetByIdAsync(id, ct);
                if (schoolClass == null || schoolClass.IsDeleted || !schoolClass.IsActive)
                {
                    throw new KeyNotFoundException($"Active class '{id}' was not found.");
                }
            }
        }

        private async Task EnsureCourseIdsAsync(List<string> ids, CancellationToken ct)
        {
            foreach (var id in ids)
            {
                var course = await _courseRepository.GetByIdAsync(id, ct);
                if (course == null || course.IsDeleted || !course.IsActive)
                {
                    throw new KeyNotFoundException($"Active course '{id}' was not found.");
                }
            }
        }

        private async Task<IReadOnlyList<string>> ResolveStudentsByDepartmentIdsAsync(string? scopeValue, CancellationToken ct)
        {
            var ids = AnnouncementScopeStorage.ParseTargetIds(scopeValue);
            var all = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var id in ids)
            {
                var students = await _studentClassRepository.GetActiveStudentIdsByDepartmentIdAsync(id, ct);
                foreach (var studentId in students)
                {
                    all.Add(studentId);
                }
            }

            return all.ToList();
        }

        private async Task<IReadOnlyList<string>> ResolveStudentsByClassIdsAsync(string? scopeValue, CancellationToken ct)
        {
            var ids = AnnouncementScopeStorage.ParseTargetIds(scopeValue);
            var all = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var id in ids)
            {
                var students = await _studentClassRepository.GetActiveStudentIdsByClassIdAsync(id, ct);
                foreach (var studentId in students)
                {
                    all.Add(studentId);
                }
            }

            return all.ToList();
        }

        private async Task<IReadOnlyList<string>> ResolveStudentsByCourseIdsAsync(string? scopeValue, CancellationToken ct)
        {
            var ids = AnnouncementScopeStorage.ParseTargetIds(scopeValue);
            var all = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var id in ids)
            {
                var students = await _courseStudentRepository.GetActiveStudentIdsByCourseIdAsync(id, ct);
                foreach (var studentId in students)
                {
                    all.Add(studentId);
                }
            }

            return all.ToList();
        }
    }
}
