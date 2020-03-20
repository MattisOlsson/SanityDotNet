namespace SanityDotNet.QueryApi.Filters
{
    public class NotFilter : Filter
    {
        public IComparisonFilter Filter { get; }

        public NotFilter(IComparisonFilter filter)
        {
            filter.ComparisonOperator = ComparisonOperator.NotEquals;
            Filter = filter;
        }

        public override string ToString()
        {
            return Filter.ToString();
        }
    }
}