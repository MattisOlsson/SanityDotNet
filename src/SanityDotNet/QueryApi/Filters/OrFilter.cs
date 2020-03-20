using System.Collections.Generic;
using System.Linq;

namespace SanityDotNet.QueryApi.Filters
{
    public class OrFilter : Filter, IFilterContainer
    {
        public OrFilter(IList<Filter> filters)
        {
            Filters = filters;
        }

        public IList<Filter> Filters { get; }

        public override string ToString()
        {
            return $"{string.Join(" || ", Filters.Select(x => x.ToString()))}";
        }
    }
}