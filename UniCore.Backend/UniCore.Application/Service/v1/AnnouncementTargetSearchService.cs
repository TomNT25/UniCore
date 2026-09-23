using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Announcement.Targets;
using UniCore.Helper.Constant;

namespace UniCore.Application.Service.v1
{
    public class AnnouncementTargetSearchService : IAnnouncementTargetSearchService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;

        public AnnouncementTargetSearchService(
            ICourseRepository courseRepository,
            ISchoolClassRepository schoolClassRepository,
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository)
        {
            _courseRepository = courseRepository;
            _schoolClassRepository = schoolClassRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        public async Task<AnnouncementTargetSearchResponseDto<CourseTargetItemDto>> SearchCoursesAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default)
        {
            var limit = AnnouncementTargetLimit.Normalize(query.Limit);
            var search = NormalizeSearch(query.Search);
            var (items, total) = await _courseRepository.SearchActiveAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<CourseTargetItemDto>
            {
                Data = items.Select(c => new CourseTargetItemDto
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    CourseCode = c.Code
                }).ToList(),
                Meta = BuildMeta(total, limit, search, AnnouncementTargetConstants.Domain.Course)
            };
        }

        public async Task<AnnouncementTargetSearchResponseDto<ClassTargetItemDto>> SearchClassesAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default)
        {
            var limit = AnnouncementTargetLimit.Normalize(query.Limit);
            var search = NormalizeSearch(query.Search);
            var (items, total) = await _schoolClassRepository.SearchActiveAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<ClassTargetItemDto>
            {
                Data = items.Select(c => new ClassTargetItemDto
                {
                    ClassId = c.Id,
                    ClassName = c.Name,
                    ClassCode = c.Code
                }).ToList(),
                Meta = BuildMeta(total, limit, search, AnnouncementTargetConstants.Domain.Class)
            };
        }

        public async Task<AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>> SearchDepartmentsAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default)
        {
            var limit = AnnouncementTargetLimit.Normalize(query.Limit);
            var search = NormalizeSearch(query.Search);
            var (items, total) = await _departmentRepository.SearchActiveAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>
            {
                Data = items.Select(d => new DepartmentTargetItemDto
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    DepartmentCode = d.Code
                }).ToList(),
                Meta = BuildMeta(total, limit, search, AnnouncementTargetConstants.Domain.Department)
            };
        }

        public async Task<AnnouncementTargetSearchResponseDto<StudentTargetItemDto>> SearchStudentsAsync(
            AnnouncementTargetSearchQuery query,
            CancellationToken cancellationToken = default)
        {
            var limit = AnnouncementTargetLimit.Normalize(query.Limit);
            var search = NormalizeSearch(query.Search);
            var (items, total) = await _userRepository.SearchActiveVerifiedStudentsAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<StudentTargetItemDto>
            {
                Data = items.Select(u => new StudentTargetItemDto
                {
                    StudentId = u.Id,
                    StudentName = ResolveStudentName(u),
                    StudentCode = u.Code ?? u.Username
                }).ToList(),
                Meta = BuildMeta(total, limit, search, AnnouncementTargetConstants.Domain.Students)
            };
        }

        private static string? NormalizeSearch(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return null;
            }

            return search.Trim();
        }

        private static AnnouncementTargetSearchMetaDto BuildMeta(int total, int limit, string? search, string domain)
        {
            return new AnnouncementTargetSearchMetaDto
            {
                Total = total,
                Limit = limit,
                Search = search,
                Domain = domain
            };
        }

        private static string ResolveStudentName(UniCore.Application.Entity.User user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserProfile?.FullName))
            {
                return user.UserProfile.FullName;
            }

            var first = user.UserProfile?.FirstName;
            var last = user.UserProfile?.LastName;
            if (!string.IsNullOrWhiteSpace(first) || !string.IsNullOrWhiteSpace(last))
            {
                return $"{first} {last}".Trim();
            }

            return user.Username;
        }
    }
}
