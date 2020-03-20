using System;
using System.Collections.Generic;
using System.Linq;

namespace SanityDotNet.QueryApi.Filters
{
    public abstract class Filter
    {
        public static Filter operator &(Filter first, Filter second)
        {
            return new AndFilter(GetFilters<AndFilter>(first, second));
        }

        public static Filter operator |(Filter first, Filter second)
        {
            return new OrFilter(GetFilters<OrFilter>(first, second));
        }

        public static Filter operator !(Filter filter)
        {
            if (filter is IComparisonFilter comparisonFilter)
            {
                return new NotFilter(comparisonFilter);
            }

            throw new ArgumentException("Filter does not implement ISanityComparisonFilter.");
        }

        private static IList<Filter> GetFilters<T>(Filter first, Filter second)
            where T : class, IFilterContainer
        {
            var filterList = new List<Filter>();

            var firstContainer = first as T;
            if (firstContainer != null)
            {
                filterList.AddRange(firstContainer.Filters.Select(filter => filter));
                filterList.Add(second);
                return filterList;
            }

            var secondContainer = second as T;
            if (secondContainer != null)
            {
                filterList.AddRange(secondContainer.Filters.Select(filter => filter));
                filterList.Insert(0, first);
                return filterList;
            }

            return new[]
            {
                first,
                second
            };
        }
    }
}