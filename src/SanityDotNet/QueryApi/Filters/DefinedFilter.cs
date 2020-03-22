namespace SanityDotNet.QueryApi.Filters
{
    public class DefinedFilter : Filter, IComparisonFilter
    {
        public string Field { get; }
        public ComparisonOperator ComparisonOperator { get; set; }

        public DefinedFilter(string field)
        {
            Field = field;
            ComparisonOperator = ComparisonOperator.Equals;
        }

        public override string ToString()
        {
            return ComparisonOperator == ComparisonOperator.Equals
                ? $"defined({Field})"
                : $"!defined({Field})";
        }

    }
}