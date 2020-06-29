using System.Collections.Generic;

namespace SanityDotNet.Web.Test.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}