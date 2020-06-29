using SanityDotNet.DataAnnotations;
using SanityDotNet.Models;

namespace SanityDotNet.Web.Test.Models
{
    [SanityContentType("productVariant")]
    public class Variant : SanityDocument
    {
        public string Title { get; set; }
        public bool IsPrimary { get; set; }
    }
}