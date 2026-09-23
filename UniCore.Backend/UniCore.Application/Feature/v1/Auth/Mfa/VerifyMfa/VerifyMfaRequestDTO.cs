using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Mfa.VerifyMfa
{
    public class VerifyMfaRequestDTO : IRequest<VerifyMfaResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
