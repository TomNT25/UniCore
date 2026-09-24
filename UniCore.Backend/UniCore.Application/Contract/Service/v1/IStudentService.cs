using UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllStudents;
using UniCore.Application.Feature.v1.Classes.GetClasses;
using UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetClassFriends;
using UniCore.Application.Feature.v1.ClassRoom.GetClassInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents;
using UniCore.Application.Feature.v1.Courses.GetAllCourses;
using UniCore.Application.Feature.v1.Courses.GetMyCourses;
using UniCore.Application.Feature.v1.Courses.GetMyCourses.PersonalDetails;
using UniCore.Application.Feature.v1.User.GetUserInfo;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IStudentService
    {
        Task <GetUserInfoResponseDTO> GetUserInfoAsync(GetUserInfoRequestDTO request, CancellationToken cancellationToken = default);

        Task <GetClassFriendsResponseDTO> GetClassmateListAsync(GetClassFriendsRequestDTO request, CancellationToken cancellationToken = default);

        Task <GetClassInfoResponseDTO> GetClassInfoAsync(GetClassInfoRequestDTO request, CancellationToken cancellationToken = default);

        Task <GetClassCourseInfosResponseDTO> GetCourseInfosAsync(GetClassCourseInfosRequestDTO request, CancellationToken cancellationToken = default);

        Task<GetCoursesStudentsResponseDTO> GetStudentCoursesAsync(GetCoursesStudentsRequestDTO request, CancellationToken cancellationToken = default);

        Task<GetAllCoursesResponseDTO> GetCoursesByName(GetAllCoursesRequestDTO request, CancellationToken cancellationToken = default);

        Task<GetClassesResponseDTO> GetClassesByName(GetClassesRequestDTO request, CancellationToken cancellationToken = default);
        Task <GetAllStudentsResponseDTO> GetClassmateListAsync(GetAllStudentsRequestDTO request, CancellationToken ct = default);
        Task<GetMyCoursesResponseDTO> GetMyCoursesAsync(GetMyCoursesRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetCourseDetailsResponseDTO> GetCourseDetailsAsync(GetCourseDetailsRequestDTO request, CancellationToken cancellationToken = default);
    }
}
