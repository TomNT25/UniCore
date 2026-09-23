using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Contract.RequestHandlerHub
{
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}