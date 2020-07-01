using System.Collections.Generic;
using MediatR;
using SanityDotNet.Models;
using SanityDotNet.Mvc.Models;

namespace SanityDotNet.Mvc.Routing
{
    public class TemplateResolvingNotification : INotification
    {
        public ISanityDocument Content { get; }
        public ContentControllerDescriptor ResolvedTemplate { get; set; }
        public IEnumerable<ContentControllerDescriptor> AvailableTemplates { get; }

        public TemplateResolvingNotification(
            ISanityDocument content,
            ContentControllerDescriptor resolvedTemplate,
            IEnumerable<ContentControllerDescriptor> availableTemplates)
        {
            Content = content;
            ResolvedTemplate = resolvedTemplate;
            AvailableTemplates = availableTemplates;
        }
    }
}