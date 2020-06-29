using System;
using SanityDotNet.Mvc.Models;

namespace SanityDotNet.Mvc.Reflection
{
    public interface ISanityTypeScanner
    {
        void Scan();
        Type[] GetContentTypes();
        ContentControllerDescriptor[] GetControllerDescriptors();
        Type GetContentType(string typeName);
    }
}