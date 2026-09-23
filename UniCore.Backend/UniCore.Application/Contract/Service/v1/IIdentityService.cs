using UniCore.Application.Feature.v1.Identity.GetMyPersonId;
using UniCore.Application.Feature.v1.Identity.ScanCccd;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IIdentityService
    {
        Task<ScanCccdResponseDTO> ScanCccdAsync(ScanCccdRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetMyPersonIdResponseDTO> GetMyPersonIdAsync(GetMyPersonIdRequestDTO request, CancellationToken cancellationToken = default);
    }
}
