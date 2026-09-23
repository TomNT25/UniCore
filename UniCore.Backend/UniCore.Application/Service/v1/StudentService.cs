using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.User.GetUserInfo;


namespace UniCore.Application.Service.v1
{
    public sealed class StudentService : IStudentService
    {
        private readonly GetUserInfoHandler _getUserInfoHandler;

        public StudentService(GetUserInfoHandler getUserInfoHandler)
        {
            _getUserInfoHandler = getUserInfoHandler;
        }

        public async Task<GetUserInfoResponseDTO?> GetUserInfoAsync(GetUserInfoRequestDTO request, CancellationToken ct)
            => await _getUserInfoHandler.HandleAsync(request, ct); 
    }
}
