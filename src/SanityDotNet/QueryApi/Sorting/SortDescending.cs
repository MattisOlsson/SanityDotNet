namespace SanityDotNet.QueryApi.Sorting
{
    public class SortDescending : SortBase
    {
        public SortDescending(string field) : base(field)
        {
        }

        protected override SortDirection SortDirection => SortDirection.Descending;
    }
}