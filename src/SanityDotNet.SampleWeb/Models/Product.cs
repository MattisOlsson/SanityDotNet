using System.Collections.Generic;
using SanityDotNet.DataAnnotations;
using SanityDotNet.Models;
using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.SampleWeb.Models
{
    [SanityContentType("product")]
    public class Product : SanityDocument
    {
        public string Title { get; set; }

        public LocaleString DisplayName { get; set; }

        public LocaleString Blurb { get; set; }

        public Reference MainCategory { get; set; }

        public override Reference Parent => MainCategory;

        public List<Reference> Categories { get; set; }

        public LocaleBlockContent Body { get; set; }
    }
}