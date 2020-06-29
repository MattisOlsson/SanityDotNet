using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SanityDotNet.Models.FieldTypes
{
    public class Block : Field, IHtmlContent
    {
        public Block()
        {
            Children = new List<BlockFragment>();
        }

        public List<BlockFragment> Children { get; set; }

        public int Level { get; set; }

        public string ListItem { get; set; }

        public string Style { get; set; }

        public bool IsListItem()
        {
            return !string.IsNullOrEmpty(ListItem);
        }

        public void WriteTo(TagBuilder wrapper, TextWriter writer, HtmlEncoder encoder)
        {
            if (Children.Count == 0)
            {
                return;
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            var tagBuilderInfo = ParseStyle();

            if (wrapper != null)
            {
                WriteToWrapper(wrapper, tagBuilderInfo);
            }
            else
            {
                var tagBuilder = CreateTagBuilder(tagBuilderInfo);

                foreach (var field in Children)
                {
                    tagBuilder.InnerHtml.AppendHtml(field);
                }

                tagBuilder.WriteTo(writer, encoder);
            }
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            WriteTo(null, writer, encoder);
        }

        protected virtual void WriteToWrapper(TagBuilder wrapper, TagBuilderInfo tagBuilderInfo)
        {
            foreach (var field in Children)
            {
                IHtmlContent html;

                if (tagBuilderInfo.NeedsWrapper)
                {
                    var tagBuilder = CreateTagBuilder(tagBuilderInfo);
                    tagBuilder.InnerHtml.AppendHtml(field);
                    html = tagBuilder;
                }
                else
                {
                    html = field;
                }

                wrapper.InnerHtml.AppendHtml(html);
            }
        }

        protected virtual TagBuilder CreateTagBuilder(TagBuilderInfo tagBuilderInfo)
        {
            var tagBuilder = new TagBuilder(tagBuilderInfo.TagName);

            if (tagBuilderInfo.CssClass != null)
            {
                tagBuilder.AddCssClass(tagBuilderInfo.CssClass);
            }

            return tagBuilder;
        }

        protected virtual TagBuilderInfo ParseStyle()
        {
            return new TagBuilderInfo(Style);
        }
    }
}