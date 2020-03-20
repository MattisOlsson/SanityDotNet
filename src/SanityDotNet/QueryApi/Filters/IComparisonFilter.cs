namespace SanityDotNet.QueryApi.Filters
{
    public interface IComparisonFilter
    {
        ComparisonOperator ComparisonOperator { get; set; }
    }
}