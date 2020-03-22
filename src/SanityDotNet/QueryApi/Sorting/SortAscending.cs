namespace SanityDotNet.QueryApi.Sorting
{
    public class SortAscending : SortBase
    {
        public SortAscending(string field) : base(field)
        {
        }

        protected override SortDirection SortDirection => SortDirection.Ascending;
    }
}