namespace SanityDotNet.Models
{
    public class Product : SanityDocument
    {
        public string Title { get; set; }
        public Variant Variant { get; set; }
        public bool IsNew { get; set; }
    }
}