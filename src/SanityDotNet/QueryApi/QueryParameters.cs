namespace SanityDotNet.QueryApi
{
    public class QueryParameters
    {
        public IQueryBuilder Query { get; set; }

        public bool Explain { get; set; }
    }
}