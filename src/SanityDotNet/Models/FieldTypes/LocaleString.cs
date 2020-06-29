using Newtonsoft.Json;
using SanityDotNet.Models.Converters;

namespace SanityDotNet.Models.FieldTypes
{
    [JsonConverter(typeof(LocaleConverter<LocaleString>))]
    public class LocaleString : Locale<string>
    {
        public override string ToString()
        {
            return Current;
        }
    }
}