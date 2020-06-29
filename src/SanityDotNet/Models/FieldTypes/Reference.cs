using System;
using Newtonsoft.Json;
using Refit;

namespace SanityDotNet.Models.FieldTypes
{
    public class Reference : Field
    {
        [AliasAs("_ref")]
        [JsonProperty("_ref")]
        public Guid Id { get; set; }
    }
}