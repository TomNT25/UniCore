using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Contract.RequestHandlerHub
{
    public interface IDispatcher
    {
        Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
    }
}