using System;

namespace SanityDotNet.Mvc
{
    public static class ControllerExtensions
    {
        internal static string GetControllerName(this Type controllerType)
        {
            var controllerSuffix = "Controller";
            var name = controllerType.Name;
            return !name.EndsWith(controllerSuffix, StringComparison.InvariantCultureIgnoreCase)
                ? name
                : name.Substring(0, name.LastIndexOf(controllerSuffix, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}