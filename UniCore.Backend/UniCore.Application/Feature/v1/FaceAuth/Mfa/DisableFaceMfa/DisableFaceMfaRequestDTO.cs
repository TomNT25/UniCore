using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.DisableFaceMfa
{
    public class DisableFaceMfaRequestDTO : IRequest<DisableFaceMfaResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
