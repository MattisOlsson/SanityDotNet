using System;
using System.Linq;
using MediatR;
using SanityDotNet.Models;
using SanityDotNet.Mvc.Models;
using SanityDotNet.Mvc.Reflection;
using SanityDotNet.Mvc.Routing;

namespace SanityDotNet.Mvc
{
    public class SanityTemplateResolver : ISanityTemplateResolver
    {
        private readonly ISanityTypeScanner _typeScanner;
        private readonly IMediator _mediator;

        public SanityTemplateResolver(ISanityTypeScanner typeScanner, IMediator mediator)
        {
            _typeScanner = typeScanner;
            _mediator = mediator;
        }

        public virtual ContentControllerDescriptor Resolve(ISanityDocument document)
        {
            var controllerDescriptors = _typeScanner
                .GetControllerDescriptors()
                .Where(x => x.ForContentType.IsInstanceOfType(document))
                .ToList();
            var resolved = controllerDescriptors.FirstOrDefault();

            var notification = new TemplateResolvingNotification(document, resolved, controllerDescriptors);
            _mediator.Publish(notification);
            return notification.ResolvedTemplate;
        }
    }
}