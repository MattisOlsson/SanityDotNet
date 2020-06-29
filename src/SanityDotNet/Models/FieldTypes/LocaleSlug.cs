using Newtonsoft.Json;
using SanityDotNet.Models.Converters;

namespace SanityDotNet.Models.FieldTypes
{
    [JsonConverter(typeof(LocaleConverter<LocaleSlug>))]
    public class LocaleSlug : Locale<Slug>
    {
        public static implicit operator string(LocaleSlug slug)
        {
            return slug?.Current;
        }

        public override string ToString()
        {
            return Current;
        }
    }
}