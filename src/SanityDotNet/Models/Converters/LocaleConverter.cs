using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.Models.Converters
{
    public class LocaleConverter<TSource> : JsonConverter where TSource : ILocaleField, new()
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var token = JToken.Load(reader);
            var locale = new TSource { Type = token.First.ToString()};
            token.First.Remove();

            serializer.Populate(token.CreateReader(), locale);
            return locale;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TSource);
        }
    }
}