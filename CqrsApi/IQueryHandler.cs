using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Queries;

namespace CqrsApi
{
    public interface IQueryHandler<in T, TResult>
        where T : IQuery<TResult>
    {
        Task<TResult> Handle(T query, CancellationToken cancellationToken);
    }
}
