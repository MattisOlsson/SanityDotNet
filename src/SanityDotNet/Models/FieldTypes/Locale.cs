using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Refit;

namespace SanityDotNet.Models.FieldTypes
{
    public abstract class Locale<TValue> : Dictionary<string, TValue>, ILocaleField
    {
        [AliasAs("_type")]
        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonIgnore] public TValue Current => GetCurrentValue();

        private TValue GetCurrentValue()
        {
            var languageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            return ContainsKey(languageName)
                ? this[languageName]
                : default;
        }

        public static implicit operator TValue(Locale<TValue> c)
        {
            return c != null ? c.Current : default;
        }
    }
}