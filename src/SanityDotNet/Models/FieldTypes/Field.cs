using Newtonsoft.Json;
using Refit;

namespace SanityDotNet.Models.FieldTypes
{
    public interface IField
    {
        string Type { get; }
    }

    public abstract class Field : IField
    {
        [AliasAs("_type")]
        [JsonProperty("_type")]
        public string Type { get; set; }
    }
}