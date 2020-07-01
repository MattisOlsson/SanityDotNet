using System.Globalization;
using SanityDotNet.Expressions;

namespace SanityDotNet.QueryApi.Filters
{
    public class SlugFilter : FieldFilter
    {
        public SlugFilter(string field, FieldFilterValue value) : base(field, value)
        {
        }

        public override string ToString()
        {
            return
                $"{Field}.{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}.current{GetComparisonOperator()}{Value.ToQueryFormat()}";
        }
    }
}