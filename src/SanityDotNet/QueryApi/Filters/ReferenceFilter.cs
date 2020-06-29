using System;

namespace SanityDotNet.QueryApi.Filters
{
    public class ReferenceFilter : FieldFilter
    {
        public ReferenceFilter(string field, Guid valueToMatch) : base(field, valueToMatch)
        {
        }

        public override string ToString()
        {
            return $"{Field}._ref{GetComparisonOperator()}{Value.ToQueryFormat()}";
        }
    }
}