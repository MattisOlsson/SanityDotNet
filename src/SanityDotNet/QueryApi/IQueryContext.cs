namespace SanityDotNet.QueryApi
{
    public interface IQueryContext
    {
        IQueryBuilder Query { get; set; }
    }

    public class QueryContext : IQueryContext
    {
        public IQueryBuilder Query { get; set; }
    }
}