using System;
using System.Reflection;

namespace SanityDotNet.Reflection
{
    public static class MemberInfoExtensions
    {
        public static Type GetMemberReturnType(this MemberInfo member)
        {
            if ((object) (member as PropertyInfo) != null)
                return ((PropertyInfo) member).PropertyType;
            return (object) (member as MethodInfo) != null ? ((MethodInfo) member).ReturnType : ((FieldInfo) member).FieldType;
        }
    }
}