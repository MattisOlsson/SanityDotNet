using System.Collections.Generic;
using Refit;

namespace SanityDotNet.QueryApi
{
    public class QueryResponse<T>
    {
        [AliasAs("ms")] public long Duration { get; set; }

        public string Query { get; set; }

        public IEnumerable<T> Result { get; set; }
    }
}