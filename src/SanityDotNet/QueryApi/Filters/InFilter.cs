using System.Collections.Generic;
using System.Linq;
using SanityDotNet.Expressions;

namespace SanityDotNet.QueryApi.Filters
{
    public class InFilter : Filter
    {
        public string Field { get; }
        public IEnumerable<FieldFilterValue> ValuesToMatch { get; }
        public string FieldSuffix { get; set; }

        public InFilter(string field, IEnumerable<FieldFilterValue> valuesToMatch)
        {
            Field = field;
            ValuesToMatch = valuesToMatch;
            FieldSuffix = string.Empty;
        }

        public override string ToString()
        {
            return $"{Field} in [{string.Join(", ", ValuesToMatch.Select(v => v.ToQueryFormat()))}]";
        }
    }
}