namespace SanityDotNet.Models
{
    public class Variant : SanityDocument
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsPrimary { get; set; }
    }
}