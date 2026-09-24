using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.FaceAuth.Mfa.EnableFaceMfa
{
    public class EnableFaceMfaRequestDTO : IRequest<EnableFaceMfaResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
