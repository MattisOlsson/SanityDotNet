namespace SanityDotNet.QueryApi.Filters
{
    public class BoolFilter : Filter, IComparisonFilter
    {
        public string Field { get; }
        public bool Value { get; }

        public BoolFilter(string field, bool value)
        {
            Field = field;
            Value = value;
        }

        public ComparisonOperator ComparisonOperator { get; set; }

        public override string ToString()
        {
            if (ComparisonOperator == ComparisonOperator.Equals)
            {
                return Value ? Field : $"!{Field}";
            }

            return Value ? $"!{Field}" : Field;
        }
    }
}