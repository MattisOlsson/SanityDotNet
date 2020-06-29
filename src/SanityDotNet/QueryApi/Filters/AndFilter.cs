using System.Collections.Generic;
using System.Linq;

namespace SanityDotNet.QueryApi.Filters
{
    public class AndFilter : Filter, IFilterContainer
    {
        public AndFilter(IList<Filter> filters)
        {
            Filters = filters;
        }

        public IList<Filter> Filters { get; }

        public override string ToString()
        {
            return $"{string.Join("&&", Filters.Select(x => x.ToString()))}";
        }
    }
}