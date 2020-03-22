using System.Collections.Generic;
using System.Linq;

namespace SanityDotNet.QueryApi.Sorting
{
    public class SortList : Sort
    {
        private readonly IList<Sort> _sortings;

        public SortList()
        {
            _sortings = new List<Sort>();
        }

        public void Add(Sort sort)
        {
            _sortings.Add(sort);
        }

        public override string ToString()
        {
            return $"order({string.Join(", ", _sortings.Select(x => x.ToString()))})";
        }
    }
}