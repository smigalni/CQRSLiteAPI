using CQRSlite.Queries;

namespace CqrsApi
{
    public class GetSomethingQuery : IQuery<GetSomethingQueryResponse>
    {
        public int MyProperty { get; set; }
    }
}