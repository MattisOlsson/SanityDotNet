using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace SanityDotNet.Models.FieldTypes
{
    public class BlockFragment : Field, IHtmlContent
    {
        public BlockFragment()
        {
            Marks = new List<string>();
        }

        [JsonProperty("_key")]
        public string Key { get; set; }

        public string Text { get; set; }

        public List<string> Marks { get; set; }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            TagBuilder marksTagBuilder = null;
            TagBuilder currentTagBuilder = null;

            foreach (var tagBuilder in Marks.Select(mark => new TagBuilder(mark)))
            {
                if (marksTagBuilder == null)
                {
                    marksTagBuilder = tagBuilder;
                }
                else
                {
                    marksTagBuilder.InnerHtml.AppendHtml(tagBuilder);
                }

                currentTagBuilder = tagBuilder;
            }

            if (currentTagBuilder != null)
            {
                currentTagBuilder.InnerHtml.Append(Text);
                marksTagBuilder.WriteTo(writer, encoder);
            }
            else
            {
                writer.Write(encoder.Encode(Text));
            }
        }
    }
}