using FluentValidation.Validators;
using System.Data.SqlTypes;
using UniCore.Application.Contract.Service.v1;
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
// using UniCore.Application.Feature.v1.User.PutUserInfo;


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
        private readonly GetMyCoursesHandler _getMyCoursesHandler;
        private readonly GetCourseDetailsHandler _getCourseDetailsHandler;
        // private readonly PutUserInfoHandler _putUserInfoHandler;

        public StudentService(
            GetUserInfoHandler getUserInfoHandler,
            GetClassFriendsHandler getClassFriendsHandler,
            GetClassInfoHandler getClassInfoHandler,
            GetClassCourseInfosHandler getClassCourseInfosHandler,
            GetCoursesStudentsHandler getCoursesStudentsHandler,
            GetAllCoursesNameHandler getAllCoursesNameHandler,
            GetClassesByNamesHandler getAllClassesByNamesHandler,
            GetMyCoursesHandler getMyCoursesHandler,
            GetCourseDetailsHandler getCourseDetailsHandler
            // PutUserInfoHandler putUserInfoHandler
            )
        {
            _getUserInfoHandler = getUserInfoHandler;
            _getClassInfoHandler = getClassInfoHandler;
            _getClassFriendsHandler = getClassFriendsHandler;
            _getClassCourseInfosHandler = getClassCourseInfosHandler;
            _getCoursesStudentsHandler = getCoursesStudentsHandler;
            _getAllCoursesNameHandler = getAllCoursesNameHandler;
            _getAllClassesByNamesHandler = getAllClassesByNamesHandler;
            _getMyCoursesHandler = getMyCoursesHandler;
            _getCourseDetailsHandler = getCourseDetailsHandler;
            // _putUserInfoHandler = putUserInfoHandler;
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
        public async Task<GetMyCoursesResponseDTO> GetMyCoursesAsync(GetMyCoursesRequestDTO request, 
            CancellationToken cancellationToken = default)
            => await _getMyCoursesHandler.HandleAsync(request, cancellationToken);
        public async Task<GetCourseDetailsResponseDTO> GetCourseDetailsAsync(GetCourseDetailsRequestDTO request,
            CancellationToken cancellationToken = default)
            => await _getCourseDetailsHandler.HandleAsync(request, cancellationToken);

        public Task<GetAllStudentsResponseDTO> GetClassmateListAsync(GetAllStudentsRequestDTO request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        // public async Task<PutUserInfoResponseDTO> PutUserInfoAsync(PutUserInfoRequestDTO request,
        //     CancellationToken cancellationToken = default)
        //     => await _putUserInfoHandler.HandleAsync(request, cancellationToken);

    }

}
