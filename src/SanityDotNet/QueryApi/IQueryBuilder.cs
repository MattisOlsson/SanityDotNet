using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using SanityDotNet.Client;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Projections;
using SanityDotNet.QueryApi.Sorting;

namespace SanityDotNet.QueryApi
{
    public interface IQueryBuilder
    {
        Filter Filter { get; set; }
        SortList Order { get; set; }
        CultureInfo Language { get; set; }
        List<Projection> Projections { get; set; }
        ISanityClient Client { get; }
        void ApplyActions(IQueryContext context);
        string ToQueryString();
        string ToEncodedQueryString();
    }

    public interface IQueryBuilder<T> : IQueryBuilder
    {
    }
}