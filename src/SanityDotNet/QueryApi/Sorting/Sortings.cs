using System;

namespace SanityDotNet.QueryApi.Sorting
{
    public static class Sortings
    {
        public static DelegateSort Ascending(this object value)
        {
            return new DelegateSort(field => new SortAscending(field));
        }

        public static DelegateSort Ascending(this int value)
        {
            return new DelegateSort(field => new SortAscending(field));
        }

        public static DelegateSort Ascending(this decimal value)
        {
            return new DelegateSort(field => new SortAscending(field));
        }

        public static DelegateSort Ascending(this float value)
        {
            return new DelegateSort(field => new SortAscending(field));
        }

        public static DelegateSort Ascending(this double value)
        {
            return new DelegateSort(field => new SortAscending(field));
        }

        public static DelegateSort Ascending(this DateTime? value)
        {
            return new DelegateSort(field => new SortAscending(field));
        }

        public static DelegateSort Descending(this object value)
        {
            return new DelegateSort(field => new SortDescending(field));
        }

        public static DelegateSort Descending(this DateTime value)
        {
            return new DelegateSort(field => new SortDescending(field));
        }

        public static DelegateSort Descending(this DateTime? value)
        {
            return new DelegateSort(field => new SortDescending(field));
        }

        public static DelegateSort Descending(this int value)
        {
            return new DelegateSort(field => new SortDescending(field));
        }
    }
}