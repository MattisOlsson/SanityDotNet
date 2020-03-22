using System;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet
{
    public class DelegateFilter : Filter, IDelegateFilter<Filter>
    {
        private readonly Func<string, Filter> _filterDelegate;

        public DelegateFilter(Func<string, Filter> filterDelegate)
        {
            _filterDelegate = filterDelegate;
        }

        public virtual Filter GetFilter(string field)
        {
            return _filterDelegate(field);
        }
    }
}