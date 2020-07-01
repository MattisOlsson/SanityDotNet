using System.Collections.Generic;

namespace SanityDotNet.SampleWeb.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }

        public string Url { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}