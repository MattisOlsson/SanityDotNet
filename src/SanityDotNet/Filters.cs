using System;
using System.Collections.Generic;
using System.Linq;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet
{
    public static class Filters
    {
        public static DelegateFilter IsDefined(this object value)
        {
            return new DelegateFilter(field => new DefinedFilter(field));
        }

        public static DelegateFilter Is(this string value, string valueToMatch)
        {
            return new DelegateFilter(fieldName => new FieldFilter(fieldName, valueToMatch));
        }

        public static DelegateFilter Is(this int value, int valueToMatch)
        {
            return new DelegateFilter(field => new FieldFilter(field, valueToMatch));
        }

        public static DelegateFilter Is(this Guid value, int valueToMatch)
        {
            return new DelegateFilter(field => new FieldFilter(field, valueToMatch));
        }

        public static DelegateFilter Is(this bool value, bool valueToMatch)
        {
            return new DelegateFilter(field => new BoolFilter(field, valueToMatch));
        }

        public static DelegateFilter Is(this DateTime value, DateTime valueToMatch)
        {
            return new DelegateFilter(field => new FieldFilter(field, valueToMatch));
        }

        public static DelegateFilter Match(this string value, string valueToMatch)
        {
            return new DelegateFilter(field => new MatchFilter(field, valueToMatch));
        }

        public static DelegateFilter In(this string value, IEnumerable<string> valuesToMatch)
        {
            return new DelegateFilter(field => new InFilter(field, valuesToMatch.Select(FieldFilterValue.Create)));
        }

        public static DelegateFilter In(this int value, IEnumerable<int> valuesToMatch)
        {
            return new DelegateFilter(fieldName => new InFilter(fieldName, valuesToMatch.Select(FieldFilterValue.Create)));
        }
    }
}