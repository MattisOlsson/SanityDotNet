using System;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet.QueryApi.Sorting
{
    public class DelegateSort : Sort, IDelegateFilter<Sort>
    {
        private readonly Func<string, Sort> _filterDelegate;

        public DelegateSort(Func<string, Sort> filterDelegate)
        {
            _filterDelegate = filterDelegate;
        }

        public Sort GetFilter(string field)
        {
            return _filterDelegate(field);
        }
    }
}