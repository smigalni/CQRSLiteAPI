using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Queries;

namespace CqrsApi
{
    public interface IQueryProcessor
    {
        Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>;
    }
}
