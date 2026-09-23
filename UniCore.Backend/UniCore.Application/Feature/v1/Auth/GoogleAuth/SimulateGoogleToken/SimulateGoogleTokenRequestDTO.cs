using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.GoogleAuth.SimulateGoogleToken
{
    public class SimulateGoogleTokenRequestDTO : IRequest<SimulateGoogleTokenResponseDTO>
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Picture { get; set; }
        public string? Sub { get; set; }
    }
}
