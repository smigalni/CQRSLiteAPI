using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsApi
{
    public class Orchestrator : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, object[]> _eventHandlers = new();

        public Orchestrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResult>
        {
            var services = _serviceProvider.GetServices(typeof(IQueryHandler<TQuery, TResult>)).ToArray();

            if (services?.Length > 1)
            {
                throw new InvalidOperationException("There can only be one queryhandler for each query.");
            }

            return _serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResult>)) is not IQueryHandler<TQuery, TResult> service
                ? throw new InvalidOperationException($"No query handler registered for {query.GetType().FullName}")
                : service.Handle(query, cancellationToken);
        }
    }
}
