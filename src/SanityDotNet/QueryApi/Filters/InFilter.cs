using System.Collections.Generic;
using System.Linq;

namespace SanityDotNet.QueryApi.Filters
{
    public class InFilter : Filter
    {
        public string Field { get; }
        public IEnumerable<FieldFilterValue> ValuesToMatch { get; }

        public InFilter(string field, IEnumerable<FieldFilterValue> valuesToMatch)
        {
            Field = field;
            ValuesToMatch = valuesToMatch;
        }

        public override string ToString()
        {
            return $"{Field} in [{string.Join(", ", ValuesToMatch.Select(v => v.ToQueryFormat()))}]";
        }
    }
}