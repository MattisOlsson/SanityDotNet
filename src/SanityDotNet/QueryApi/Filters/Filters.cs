using System;
using System.Collections.Generic;
using System.Linq;
using SanityDotNet.Expressions;
using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.QueryApi.Filters
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

        public static DelegateFilter Is(this Guid value, Guid valueToMatch)
        {
            return new DelegateFilter(field => new FieldFilter(field, valueToMatch));
        }

        public static DelegateFilter Is(this Guid value, string valueToMatch)
        {
            return new DelegateFilter(field => new FieldFilter(field, Guid.Parse(valueToMatch)));
        }

        public static DelegateFilter Is(this bool value, bool valueToMatch)
        {
            return new DelegateFilter(field => new BoolFilter(field, valueToMatch));
        }

        public static DelegateFilter Is(this Reference value, Guid valueToMatch)
        {
            return new DelegateFilter(field => new ReferenceFilter(field, valueToMatch));
        }

        public static DelegateFilter Is(this DateTime value, DateTime valueToMatch)
        {
            return new DelegateFilter(field => new FieldFilter(field, valueToMatch));
        }

        public static DelegateFilter Is(this LocaleSlug value, string valueToMatch)
        {
            return new DelegateFilter(fieldName => new SlugFilter(fieldName, valueToMatch));
        }

        public static DelegateFilter Match(this string value, string valueToMatch)
        {
            return new DelegateFilter(field => new MatchFilter(field, valueToMatch));
        }

        public static DelegateFilter MatchContained(this IEnumerable<Reference> value, Guid valueToMatch)
        {
            return new DelegateFilter(field => new MatchContainedFilter(field, valueToMatch) {FieldSuffix = "[]._ref"});
        }

        public static DelegateFilter In(this string value, IEnumerable<string> valuesToMatch)
        {
            return new DelegateFilter(field => new InFilter(field, valuesToMatch.Select(FieldFilterValue.Create)));
        }

        public static DelegateFilter In(this Reference value, IEnumerable<Guid> valuesToMatch)
        {
            return new DelegateFilter(field => new InFilter(field, valuesToMatch.Select(FieldFilterValue.Create))
                {FieldSuffix = "._ref"});
        }

        public static DelegateFilter In(this int value, IEnumerable<int> valuesToMatch)
        {
            return new DelegateFilter(fieldName => new InFilter(fieldName, valuesToMatch.Select(FieldFilterValue.Create)));
        }
    }
}