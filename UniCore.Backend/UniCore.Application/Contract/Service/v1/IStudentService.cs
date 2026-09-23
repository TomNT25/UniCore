using UniCore.Application.Feature.v1.User.GetUserInfo;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IStudentService
    {
        Task<GetUserInfoResponseDTO?> GetUserInfoAsync(GetUserInfoRequestDTO request, CancellationToken ct);
    }
}
