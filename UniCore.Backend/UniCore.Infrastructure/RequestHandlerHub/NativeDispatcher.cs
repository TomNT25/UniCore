using UniCore.Application.Contract.RequestHandlerHub;
using Microsoft.Extensions.DependencyInjection;

namespace UniCore.Infrastructure.RequestHandlerHub
{
    public class NativeDispatcher : IDispatcher
    {
        private readonly IServiceProvider _provider;

        public NativeDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            using var scope = _provider.CreateScope();
            dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
            return await handler.HandleAsync((dynamic)request, ct);
        }
    }
}