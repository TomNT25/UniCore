using UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetClassFriends;
using UniCore.Application.Feature.v1.ClassRoom.GetClassInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents;
using UniCore.Application.Feature.v1.User.GetUserInfo;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IStudentService
    {
        Task <GetUserInfoResponseDTO> GetUserInfoAsync(GetUserInfoRequestDTO request, CancellationToken ct = default);

        Task <IEnumerable<GetAllStudentsResponseDTO>> GetClassmateListAsync(GetAllStudentsRequestDTO request, CancellationToken ct = default);

        Task <GetClassInfoResponseDTO> GetClassInfoAsync(GetClassInfoRequestDTO request, CancellationToken ct = default);

        Task <IEnumerable<GetClassCourseInfosResponseDTO>> GetCourseInfosAsync(GetClassCourseInfosRequestDTO request, CancellationToken ct = default);

        Task<IEnumerable<GetCoursesStudentsResponseDTO>> GetStudentCoursesAsync(GetCoursesStudentsRequestDTO request, CancellationToken ct = default);

    }
}
