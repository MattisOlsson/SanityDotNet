namespace SanityDotNet.QueryApi.Filters
{
    public interface IDelegateFilter<out TFilter>
    {
        TFilter GetFilter(string field);
    }
}