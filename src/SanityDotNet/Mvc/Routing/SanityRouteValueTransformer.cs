using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SanityDotNet.Extensions;
using SanityDotNet.Mvc.Routing.Segments;

namespace SanityDotNet.Mvc.Routing
{
    public class SanityRouteValueTransformer : DynamicRouteValueTransformer
    {
        private readonly SanityOptions _options;
        private readonly ISanityTemplateResolver _templateResolver;

        public SanityRouteValueTransformer(IOptions<SanityOptions> options, ISanityTemplateResolver templateResolver)
        {
            _options = options.Value;
            _templateResolver = templateResolver;
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(
            HttpContext httpContext,
            RouteValueDictionary values)
        {
            if (!httpContext.Request.Path.HasValue)
            {
                return values;
            }

            var segmentContext = new SegmentContext(httpContext.Request)
            {
                RemainingPath = httpContext.Request.Path.Value.Substring(1).TrimEnd('/'),
                Language = CultureInfo.CurrentUICulture // Default to current UI culture
            };

            var isMatch = await MatchSegments(segmentContext);

            if (!isMatch)
            {
                return values;
            }

            var controllerDescriptor = _templateResolver.Resolve(segmentContext.RoutedDocument);

            if (controllerDescriptor != null)
            {
                var action = string.IsNullOrEmpty(segmentContext.RemainingPath)
                    ? "index"
                    : segmentContext.RemainingPath;

                values[RoutingConstants.ControllerRouteValueKey] = controllerDescriptor.ControllerType.GetControllerName();
                values[RoutingConstants.ActionRouteValueKey] = action;
                values[RoutingConstants.CurrentDocumentRouteValueKey] = segmentContext.RoutedDocument;
                values[RoutingConstants.DocumentRouteValueKey] = segmentContext.RoutedDocumentId;
            }

            return values;
        }

        protected virtual async Task<bool> MatchSegments(SegmentContext segmentContext)
        {
            foreach (var segment in _options.RoutingSegments)
            {
                var isSegmentMatch = await segment.IsMatch(segmentContext);

                if (!isSegmentMatch)
                {
                    return false;
                }
            }

            return true;
        }
    }
}