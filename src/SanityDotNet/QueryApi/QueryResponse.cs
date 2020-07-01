using System.Collections.Generic;
using Newtonsoft.Json;
using Refit;

namespace SanityDotNet.QueryApi
{
    public class QueryResponse<T>
    {
        [JsonProperty("ms")] public long Duration { get; set; }

        public string Query { get; set; }

        public IEnumerable<T> Result { get; set; }
    }
}