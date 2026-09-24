using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Announcement.Targets;

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

        public async Task<SearchCourseTargetsResponseDTO> SearchCoursesAsync(
            SearchCourseTargetsRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            var pagedResult = await _courseRepository.SearchActiveCursorAsync(request, cancellationToken);

            return new SearchCourseTargetsResponseDTO
            {
                Items = pagedResult.Items.Select(c => new CourseTargetItemDto
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    CourseCode = c.Code
                }).ToList(),
                Metadata = pagedResult.Metadata
            };
        }

        public async Task<SearchClassTargetsResponseDTO> SearchClassesAsync(
            SearchClassTargetsRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            var pagedResult = await _schoolClassRepository.SearchActiveCursorAsync(request, cancellationToken);

            return new SearchClassTargetsResponseDTO
            {
                Items = pagedResult.Items.Select(c => new ClassTargetItemDto
                {
                    ClassId = c.Id,
                    ClassName = c.Name,
                    ClassCode = c.Code
                }).ToList(),
                Metadata = pagedResult.Metadata
            };
        }

        public async Task<SearchDepartmentTargetsResponseDTO> SearchDepartmentsAsync(
            SearchDepartmentTargetsRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            var pagedResult = await _departmentRepository.SearchActiveCursorAsync(request, cancellationToken);

            return new SearchDepartmentTargetsResponseDTO
            {
                Items = pagedResult.Items.Select(d => new DepartmentTargetItemDto
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    DepartmentCode = d.Code
                }).ToList(),
                Metadata = pagedResult.Metadata
            };
        }

        public async Task<SearchStudentTargetsResponseDTO> SearchStudentsAsync(
            SearchStudentTargetsRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            var pagedResult = await _userRepository.SearchActiveVerifiedStudentsCursorAsync(request, cancellationToken);

            return new SearchStudentTargetsResponseDTO
            {
                Items = pagedResult.Items.Select(u => new StudentTargetItemDto
                {
                    StudentId = u.Id,
                    StudentName = ResolveStudentName(u),
                    StudentCode = u.StudentCode ?? u.Code ?? u.Username
                }).ToList(),
                Metadata = pagedResult.Metadata
            };
        }

        private static string ResolveStudentName(User user)
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
