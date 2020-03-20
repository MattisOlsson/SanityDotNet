using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SanityDotNet.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool IsExtensionMethod(this MethodInfo methodInfo)
        {
            return (uint) methodInfo.GetCustomAttributes(typeof(ExtensionAttribute), false).Length > 0U;
        }

        public static bool HasGenericTypeDefinition(this MethodInfo methodInfo, Type genericType)
        {
            return methodInfo.ReturnType.IsGenericType
                   && genericType.IsAssignableFrom(methodInfo.ReturnType.GetGenericTypeDefinition());
        }
    }
}