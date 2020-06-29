using System;

namespace SanityDotNet.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SanityContentTypeAttribute : Attribute
    {
        public string Type { get; }

        public SanityContentTypeAttribute(string type)
        {
            Type = type;
        }
    }
}