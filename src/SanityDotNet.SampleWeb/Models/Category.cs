using SanityDotNet.DataAnnotations;
using SanityDotNet.Models;
using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.SampleWeb.Models
{
    [SanityContentType("category")]
    public class Category : SanityDocument
    {
        public string Title { get; set; }

        public LocaleString DisplayName { get; set; }

        public string Description { get; set; }
    }
}