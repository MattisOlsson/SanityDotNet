using System;
using System.Collections.Generic;
using System.Linq;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet
{
    public static class Filters
    {
        public static DelegateFilterBuilder Is(this string value, string valueToMatch)
        {
            return new DelegateFilterBuilder(fieldName => new FieldFilter(fieldName, valueToMatch));
        }

        public static DelegateFilterBuilder Is(this int value, int valueToMatch)
        {
            return new DelegateFilterBuilder(fieldName => new FieldFilter(fieldName, valueToMatch));
        }

        public static DelegateFilterBuilder Is(this Guid value, int valueToMatch)
        {
            return new DelegateFilterBuilder(fieldName => new FieldFilter(fieldName, valueToMatch));
        }

        public static DelegateFilterBuilder Is(this bool value, bool valueToMatch)
        {
            return new DelegateFilterBuilder(field => new BoolFilter(field, valueToMatch));
        }

        public static DelegateFilterBuilder Is(this DateTime value, DateTime valueToMatch)
        {
            return new DelegateFilterBuilder(fieldName => new FieldFilter(fieldName, valueToMatch));
        }
        public static DelegateFilterBuilder In(this string value, IEnumerable<string> valuesToMatch)
        {
            return new DelegateFilterBuilder(fieldName => new InFilter(fieldName, valuesToMatch.Select(FieldFilterValue.Create)));
        }

        public static DelegateFilterBuilder In(this int value, IEnumerable<int> valuesToMatch)
        {
            return new DelegateFilterBuilder(fieldName => new InFilter(fieldName, valuesToMatch.Select(FieldFilterValue.Create)));
        }
    }

    public class DelegateFilterBuilder : Filter
    {
        private readonly Func<string, Filter> _filterDelegate;

        public DelegateFilterBuilder(Func<string, Filter> filterDelegate = null)
        {
            _filterDelegate = filterDelegate;
        }

        public virtual Filter GetFilter(string fieldName)
        {
            return _filterDelegate(fieldName);
        }
    }
}