using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Mfa.EnableMfa
{
    public class EnableMfaRequestDTO : IRequest<EnableMfaResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
