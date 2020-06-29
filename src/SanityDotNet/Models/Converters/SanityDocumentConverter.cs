using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SanityDotNet.Mvc.Reflection;
using SanityDotNet.Reflection;
using SanityDotNet.Web;

namespace SanityDotNet.Models.Converters
{
    public class SanityDocumentConverter : JsonConverter
    {
        private readonly ISanityTypeScanner _sanityTypeScanner;

        public SanityDocumentConverter(ISanityTypeScanner sanityTypeScanner)
        {
            _sanityTypeScanner = sanityTypeScanner;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JToken.Load(reader);
            var typeName = obj.Value<string>("_type");
            var contentType = _sanityTypeScanner.GetContentType(typeName);

            if (contentType == null)
            {
                return null;
            }

            var document = Activator.CreateInstance(contentType);
            serializer.Populate(obj.CreateReader(), document);
            return document;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ISanityDocument).IsAssignableFrom(objectType);
        }
    }
}