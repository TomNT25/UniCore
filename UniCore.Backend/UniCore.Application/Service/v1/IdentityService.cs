using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Identity.GetMyPersonId;
using UniCore.Application.Feature.v1.Identity.ScanCccd;

namespace UniCore.Application.Service.v1
{
    public class IdentityService : IIdentityService
    {
        private readonly ScanCccdHandler _scanCccdHandler;
        private readonly GetMyPersonIdHandler _getMyPersonIdHandler;

        public IdentityService(ScanCccdHandler scanCccdHandler, GetMyPersonIdHandler getMyPersonIdHandler)
        {
            _scanCccdHandler = scanCccdHandler;
            _getMyPersonIdHandler = getMyPersonIdHandler;
        }

        public Task<ScanCccdResponseDTO> ScanCccdAsync(ScanCccdRequestDTO request, CancellationToken cancellationToken = default)
            => _scanCccdHandler.HandleAsync(request, cancellationToken);

        public Task<GetMyPersonIdResponseDTO> GetMyPersonIdAsync(GetMyPersonIdRequestDTO request, CancellationToken cancellationToken = default)
            => _getMyPersonIdHandler.HandleAsync(request, cancellationToken);
    }
}
