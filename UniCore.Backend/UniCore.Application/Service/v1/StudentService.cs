using FluentValidation.Validators;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetClassFriends;
using UniCore.Application.Feature.v1.ClassRoom.GetClassInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents;
using UniCore.Application.Feature.v1.User.GetUserInfo;


namespace UniCore.Application.Service.v1
{
    public class StudentService : IStudentService
    {
        private readonly GetUserInfoHandler _getUserInfoHandler;
        private readonly GetClassFriendsHandler _getClassFriendsHandler;
        private readonly GetClassInfoHandler _getClassInfoHandler;
        private readonly GetClassCourseInfosHandler _getClassCourseInfosHandler;
        private readonly GetCoursesStudentsHandler _getCoursesStudentsHandler;

        public StudentService(
            GetUserInfoHandler getUserInfoHandler,
            GetClassFriendsHandler getClassFriendsHandler,
            GetClassInfoHandler getClassInfoHandler,
            GetClassCourseInfosHandler getClassCourseInfosHandler,
            GetCoursesStudentsHandler getCoursesStudentsHandler
            )
        {
            _getUserInfoHandler = getUserInfoHandler;
            _getClassInfoHandler = getClassInfoHandler;
            _getClassFriendsHandler = getClassFriendsHandler;
            _getClassCourseInfosHandler = getClassCourseInfosHandler;
            _getCoursesStudentsHandler = getCoursesStudentsHandler;
        }

        public async Task<GetUserInfoResponseDTO> GetUserInfoAsync(GetUserInfoRequestDTO request, CancellationToken ct = default)
            => await _getUserInfoHandler.HandleAsync(request, ct);

        public async Task<IEnumerable<GetAllStudentsResponseDTO>> GetClassmateListAsync(GetAllStudentsRequestDTO request, CancellationToken ct = default)
            => await _getClassFriendsHandler.HandleAsync(request, ct);

        public async Task<GetClassInfoResponseDTO> GetClassInfoAsync(GetClassInfoRequestDTO request, CancellationToken ct = default)
            => await _getClassInfoHandler.HandleAsync(request, ct);

        public async Task<IEnumerable<GetClassCourseInfosResponseDTO>> GetCourseInfosAsync(GetClassCourseInfosRequestDTO request, CancellationToken ct = default)
            => await _getClassCourseInfosHandler.HandleAsync(request, ct);

        public async Task<IEnumerable<GetCoursesStudentsResponseDTO>> GetStudentCoursesAsync(GetCoursesStudentsRequestDTO request, CancellationToken ct)
            => await _getCoursesStudentsHandler.HandleAsync(request, ct);
    }

}
