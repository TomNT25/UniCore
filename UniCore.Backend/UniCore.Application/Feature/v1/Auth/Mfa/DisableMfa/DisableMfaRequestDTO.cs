using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Mfa.DisableMfa
{
    public class DisableMfaRequestDTO : IRequest<DisableMfaResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
