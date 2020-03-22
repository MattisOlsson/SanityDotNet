using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Sorting;

namespace SanityDotNet.QueryApi
{
    public interface IQueryBuilder
    {
        Filter Filter { get; set; }
        SortList Order { get; set; }
        void ApplyActions(IQueryContext context);
        string ToQueryString();
    }

    public interface IQueryBuilder<T> : IQueryBuilder
    {
    }
}