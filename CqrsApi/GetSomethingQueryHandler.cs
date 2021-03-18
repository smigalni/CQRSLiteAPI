using System.Threading;
using System.Threading.Tasks;

namespace CqrsApi
{
    public class GetSomethingQueryHandler : IQueryHandler<GetSomethingQuery, GetSomethingQueryResponse>
    {
        public Task<GetSomethingQueryResponse> Handle(GetSomethingQuery message, CancellationToken token = default)
        {
            return Task.FromResult(new GetSomethingQueryResponse { MyProperty = 5});
        }
    }
}