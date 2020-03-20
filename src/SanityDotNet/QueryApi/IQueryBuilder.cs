using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet.QueryApi
{
    public interface IQueryBuilder
    {
        Filter Filter { get; set; }
        void ApplyActions(IQueryContext context);
        string ToQueryString();
    }

    public interface IQueryBuilder<T> : IQueryBuilder
    {
    }
}