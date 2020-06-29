using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using SanityDotNet.Models.Converters;

namespace SanityDotNet.Models.FieldTypes
{
    [JsonConverter(typeof(LocaleConverter<LocaleBlockContent>))]
    public class LocaleBlockContent : Locale<BlockContent>, IHtmlContent
    {
        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            Current?.WriteTo(writer, encoder);
        }
    }
}