using System;
using Newtonsoft.Json;
using Refit;
using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.Models
{
    public interface ISanityDocument
    {
        [AliasAs("_id")]
        [JsonProperty("_id")]
        Guid Id { get; set; }

        [AliasAs("_rev")]
        [JsonProperty("_rev")]
        string Revision { get; set; }

        [AliasAs("_type")]
        [JsonProperty("_type")]
        string Type { get; set; }

        LocaleSlug Slug { get; set; }

        [AliasAs("_createdAt")]
        [JsonProperty("_createdAt")]
        DateTime CreatedAt { get; set; }

        [AliasAs("_updatedAt")]
        [JsonProperty("_updatedAt")]
        DateTime? UpdatedAt { get; set; }

        Reference Parent { get; set; }
    }
}