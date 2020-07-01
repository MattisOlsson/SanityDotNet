using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;
using Refit;
using SanityDotNet.Models.Converters;
using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.Models
{
    public abstract class SanityDocument : ISanityDocument
    {
        [AliasAs("_id")] [JsonProperty("_id")] public Guid Id { get; set; }

        [AliasAs("_rev")]
        [JsonProperty("_rev")]
        public string Revision { get; set; }

        [AliasAs("_type")]
        [JsonProperty("_type")]
        public string Type { get; set; }

        public LocaleSlug Slug { get; set; }

        [AliasAs("_createdAt")]
        [JsonProperty("_createdAt")]
        public DateTime CreatedAt { get; set; }

        [AliasAs("_updatedAt")]
        [JsonProperty("_updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        public virtual Reference Parent { get; set; }
    }
}