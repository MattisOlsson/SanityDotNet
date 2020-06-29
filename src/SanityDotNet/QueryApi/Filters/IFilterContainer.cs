using System.Collections.Generic;

namespace SanityDotNet.QueryApi.Filters
{
    public interface IFilterContainer
    {
        IList<Filter> Filters { get; }
    }
}