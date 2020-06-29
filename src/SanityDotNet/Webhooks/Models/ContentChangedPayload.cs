using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Refit;

namespace SanityDotNet.Webhooks.Models
{
    public class ContentChangedPayload
    {
        public string TransactionId { get; set; }

        public string ProjectId { get; set; }

        public string Dataset { get; set; }

        [AliasAs("ids")]
        [JsonProperty("ids")]
        [JsonPropertyName("ids")]
        public DocumentOperations DocumentIds { get; set; }

        public class DocumentOperations
        {
            public Guid[] Created { get; set; }
            public Guid[] Deleted { get; set; }
            public Guid[] Updated { get; set; }
            public Guid[] All { get; set; }
        }
    }
}