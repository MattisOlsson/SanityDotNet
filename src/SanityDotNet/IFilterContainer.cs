using System.Collections.Generic;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet
{
    public interface IFilterContainer
    {
        IList<Filter> Filters { get; }
    }
}