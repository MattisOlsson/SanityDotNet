using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SanityDotNet.DataAnnotations;
using SanityDotNet.Models;
using SanityDotNet.Mvc.Models;

namespace SanityDotNet.Mvc.Reflection
{
    public class SanityTypeScanner : ISanityTypeScanner
    {
        private readonly IDictionary<string, Type> _typeMappings;
        private readonly List<ContentControllerDescriptor> _controllerDescriptors;

        public SanityTypeScanner()
        {
            _typeMappings = new Dictionary<string, Type>();
            _controllerDescriptors = new List<ContentControllerDescriptor>();
        }

        public virtual void Scan()
        {
            var assemblies = GetAssemblies();
            ScanContentTypes(assemblies);
            ScanControllers(assemblies);
        }

        public virtual Type[] GetContentTypes()
        {
            return _typeMappings.Values.ToArray();
        }

        public virtual ContentControllerDescriptor[] GetControllerDescriptors()
        {
            return _controllerDescriptors.ToArray();
        }

        public virtual Type GetContentType(string typeName)
        {
            return _typeMappings.ContainsKey(typeName)
                ? _typeMappings[typeName]
                : null;
        }

        protected virtual Assembly[] GetAssemblies()
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies();
        }

        protected virtual void ScanContentTypes(Assembly[] assemblies)
        {
            var contentTypes = assemblies
                .SelectMany(x =>
                    x.GetTypes()
                        .Where(y => y.GetCustomAttribute<SanityContentTypeAttribute>() != null));

            foreach (var contentType in contentTypes)
            {
                var contentTypeInfo = contentType.GetCustomAttribute<SanityContentTypeAttribute>();

                if (!_typeMappings.ContainsKey(contentTypeInfo.Type))
                {
                    _typeMappings.Add(contentTypeInfo.Type, contentType);
                }
            }
        }

        protected virtual void ScanControllers(Assembly[] assemblies)
        {
            var controllerTypes = assemblies
                .SelectMany(assembly => assembly
                    .GetTypes()
                    .Where(type => !type.IsAbstract
                                   && typeof(Controller).IsAssignableFrom(type)
                                   && type.BaseType != null
                                   && type.BaseType != typeof(Controller)));

            foreach (var controllerType in controllerTypes)
            {
                var baseType = controllerType.BaseType;

                while (baseType != null && typeof(Controller).IsAssignableFrom(baseType))
                {
                    if (baseType.IsGenericType)
                    {
                        if (baseType.GenericTypeArguments.Length > 0)
                        {
                            var genericTypeTarget = baseType.GenericTypeArguments[0];

                            if (typeof(ISanityDocument).IsAssignableFrom(genericTypeTarget))
                            {
                                _controllerDescriptors.Add(new ContentControllerDescriptor
                                {
                                    ControllerType = controllerType,
                                    ForContentType = genericTypeTarget
                                });
                            }
                        }
                    }

                    baseType = baseType.BaseType;
                }
            }
        }
    }
}