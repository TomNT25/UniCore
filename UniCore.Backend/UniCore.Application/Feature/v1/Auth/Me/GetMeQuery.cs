using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.Me
{
    public record GetMeQuery(string UserId) : IRequest<GetMeResponseDTO?>;
}
