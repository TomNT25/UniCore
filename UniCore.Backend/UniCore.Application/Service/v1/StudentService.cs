using FluentValidation.Validators;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Admin.StudentsManagement.GetAllStudents;
using UniCore.Application.Feature.v1.Classes.GetClasses;
using UniCore.Application.Feature.v1.ClassRoom.GetClassCourseInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetClassFriends;
using UniCore.Application.Feature.v1.ClassRoom.GetClassInfos;
using UniCore.Application.Feature.v1.ClassRoom.GetCoursesStudents;
using UniCore.Application.Feature.v1.Courses.GetAllCourses;
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
        private readonly GetAllCoursesNameHandler _getAllCoursesNameHandler;
        private readonly GetClassesByNamesHandler _getAllClassesByNamesHandler;
    

        public StudentService(
            GetUserInfoHandler getUserInfoHandler,
            GetClassFriendsHandler getClassFriendsHandler,
            GetClassInfoHandler getClassInfoHandler,
            GetClassCourseInfosHandler getClassCourseInfosHandler,
            GetCoursesStudentsHandler getCoursesStudentsHandler,
            GetAllCoursesNameHandler getAllCoursesNameHandler,
            GetClassesByNamesHandler getAllClassesByNamesHandler
            )
        {
            _getUserInfoHandler = getUserInfoHandler;
            _getClassInfoHandler = getClassInfoHandler;
            _getClassFriendsHandler = getClassFriendsHandler;
            _getClassCourseInfosHandler = getClassCourseInfosHandler;
            _getCoursesStudentsHandler = getCoursesStudentsHandler;
            _getAllCoursesNameHandler = getAllCoursesNameHandler;
            _getAllClassesByNamesHandler = getAllClassesByNamesHandler;
        }

        public async Task<GetUserInfoResponseDTO> GetUserInfoAsync(GetUserInfoRequestDTO request, CancellationToken cancellationToken = default)
            => await _getUserInfoHandler.HandleAsync(request, cancellationToken);

        public async Task<GetClassFriendsResponseDTO> GetClassmateListAsync(GetClassFriendsRequestDTO request, CancellationToken cancellationToken = default)
            => await _getClassFriendsHandler.HandleAsync(request, cancellationToken);

        public async Task<GetClassInfoResponseDTO> GetClassInfoAsync(GetClassInfoRequestDTO request, CancellationToken cancellationToken = default)
            => await _getClassInfoHandler.HandleAsync(request, cancellationToken);

        public async Task<GetClassCourseInfosResponseDTO> GetCourseInfosAsync(GetClassCourseInfosRequestDTO request, CancellationToken cancellationToken = default)
            => await _getClassCourseInfosHandler.HandleAsync(request, cancellationToken);

        public async Task<GetCoursesStudentsResponseDTO> GetStudentCoursesAsync(GetCoursesStudentsRequestDTO request, CancellationToken cancellationToken = default)
            => await _getCoursesStudentsHandler.HandleAsync(request, cancellationToken);

        public async Task<GetAllCoursesResponseDTO> GetCoursesByName(GetAllCoursesRequestDTO request, CancellationToken cancellationToken = default)
            => await _getAllCoursesNameHandler.HandleAsync(request, cancellationToken);

        public async Task<GetClassesResponseDTO> GetClassesByName(GetClassesRequestDTO request, CancellationToken cancellationToken = default) 
            => await _getAllClassesByNamesHandler.HandleAsync(request, cancellationToken);

        public Task<GetAllStudentsResponseDTO> GetClassmateListAsync(GetAllStudentsRequestDTO request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

    }

}
