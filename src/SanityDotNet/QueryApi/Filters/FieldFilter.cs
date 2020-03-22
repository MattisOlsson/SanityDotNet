namespace SanityDotNet.QueryApi.Filters
{
    public class FieldFilter : Filter, IComparisonFilter
    {
        public FieldFilter(string field, FieldFilterValue value)
        {
            Field = field;
            Value = value;
            ComparisonOperator = ComparisonOperator.Equals;
        }

        public string Field { get; }
        public FieldFilterValue Value { get; }
        public ComparisonOperator ComparisonOperator { get; set; }

        public override string ToString()
        {
            if (Value.Is<bool>())
            {
                return ComparisonOperator == ComparisonOperator.Equals
                    ? Field
                    : $"!{Field}";
            }

            return $"{Field} {GetComparisonOperator()} {Value.ToQueryFormat()}";
        }

        protected virtual string GetComparisonOperator()
        {
            switch (ComparisonOperator)
            {
                case ComparisonOperator.NotEquals:
                    return "!=";
                case ComparisonOperator.GreaterThan:
                    return ">";
                case ComparisonOperator.GreaterThanOrEqual:
                    return ">=";
                case ComparisonOperator.LessThan:
                    return "<";
                case ComparisonOperator.LessThanOrEqual:
                    return "<=";
                default:
                    return "==";
            }
        }
    }
}