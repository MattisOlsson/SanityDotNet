namespace SanityDotNet
{
    public interface IDelegateFilter<out TFilter>
    {
        TFilter GetFilter(string field);
    }
}