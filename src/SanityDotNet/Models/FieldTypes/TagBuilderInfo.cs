namespace SanityDotNet.Models.FieldTypes
{
    public class TagBuilderInfo
    {
        public TagBuilderInfo(string style)
        {
            if (string.IsNullOrEmpty(style) || style.Equals("normal"))
            {
                TagName = "p";
                CssClass = null;
            }
            else
            {
                if (style.StartsWith('.'))
                {
                    TagName = "p";
                    CssClass = style.Substring(1);
                }
                else
                {
                    TagName = style;
                    CssClass = null;
                    NeedsWrapper = true;
                }
            }
        }

        public string TagName { get; set; }

        public string CssClass { get; set; }

        public bool NeedsWrapper { get; set; }
    }
}