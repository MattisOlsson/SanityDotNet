using System;
using System.Collections.Generic;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Sorting;

namespace SanityDotNet
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