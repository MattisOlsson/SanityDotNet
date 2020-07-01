using System.Collections.Generic;

namespace SanityDotNet.Mvc.Routing
{
    public static class RoutingExtensions
    {
        public static bool TryGetCustomValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            if (dictionary.TryGetValue(key, out var objValue))
            {
                value = (T) objValue;
                return true;
            }

            value = default;
            return false;
        }
    }
}