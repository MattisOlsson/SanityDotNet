using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Refit;

namespace SanityDotNet.Models.FieldTypes
{
    public class BlockContent : List<Block>, IHtmlContent, IField
    {
        [AliasAs("_type")]
        [JsonProperty("_type")]
        public string Type => "blockContent";

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            var isFirstListItem = true;
            TagBuilder listTagBuilder = null;

            foreach (var block in this)
            {
                if (block.IsListItem())
                {
                    var listTagName = GetListTagName(block);
                    var isDifferentTag = !listTagName.Equals(listTagBuilder?.TagName);

                    if (isFirstListItem || isDifferentTag)
                    {
                        listTagBuilder?.WriteTo(writer, encoder);
                        listTagBuilder = new TagBuilder(listTagName);
                        isFirstListItem = false;
                    }

                    var listItemTag = new TagBuilder("li");
                    var style = new TagBuilderInfo(block.Style);

                    if (style.CssClass != null)
                    {
                        listItemTag.AddCssClass(style.CssClass);
                    }

                    block.WriteTo(listItemTag, writer, encoder);
                    listTagBuilder.InnerHtml.AppendHtml(listItemTag);
                }
                else
                {
                    isFirstListItem = true;
                    listTagBuilder?.WriteTo(writer, encoder);
                    listTagBuilder = null;
                    block.WriteTo(writer, encoder);
                }
            }

            listTagBuilder?.WriteTo(writer, encoder);
        }

        protected virtual string GetListTagName(Block block)
        {
            return block.ListItem.Equals("bullet")
                ? "ul"
                : "ol";
        }
    }
}