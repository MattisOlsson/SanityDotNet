using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace SanityDotNet.Mvc.Routing
{
    public class SanityDocumentValueProvider : IValueProvider
    {
        private const string CurrentDocumentKey = "currentDocument";

        public bool ContainsPrefix(string prefix)
        {
            return CurrentDocumentKey.Equals(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            if (!ContainsPrefix(key))
            {
                return ValueProviderResult.None;
            }

            var values = new StringValues(CurrentDocumentKey);
            return new ValueProviderResult(values);
        }
    }
}