using Refit;

namespace SanityDotNet.Models
{
    public abstract class SanityDocument
    {
        [AliasAs("_type")]
        public string Type { get; set; }
        public bool IsPublished { get; set; }
    }
}