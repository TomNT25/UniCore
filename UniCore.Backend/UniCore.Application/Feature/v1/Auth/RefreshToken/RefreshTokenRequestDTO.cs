using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.RefreshToken
{
    public class RefreshTokenRequestDTO : IRequest<RefreshTokenResponseDTO>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
