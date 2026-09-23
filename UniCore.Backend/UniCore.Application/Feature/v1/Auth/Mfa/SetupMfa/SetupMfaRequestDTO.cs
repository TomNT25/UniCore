using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Mfa.SetupMfa
{
    public class SetupMfaRequestDTO : IRequest<SetupMfaResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
