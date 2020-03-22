namespace SanityDotNet.QueryApi.Sorting
{
    public abstract class SortBase : Sort
    {
        private readonly string _field;

        protected SortBase(string field)
        {
            _field = field;
        }

        protected abstract SortDirection SortDirection { get; }

        public override string ToString()
        {
            return $"{_field} {GetSortDirection()}";
        }

        protected virtual string GetSortDirection()
        {
            switch (SortDirection)
            {
                case SortDirection.Descending:
                    return "desc";
                default:
                    return "asc";
            }
        }
    }
}