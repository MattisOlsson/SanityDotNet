namespace SanityDotNet.Models.FieldTypes
{
    public class Slug : Field
    {
        public string Current { get; set; }

        public override string ToString()
        {
            return this;
        }

        public static implicit operator string(Slug slug)
        {
            return slug?.Current;
        }
    }
}