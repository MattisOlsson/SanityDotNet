using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SanityDotNet.Client;
using SanityDotNet.Models;
using SanityDotNet.Mvc;
using SanityDotNet.Mvc.Routing;

namespace SanityDotNet.Web
{
    public class UrlResolver : UrlResolverBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SanityOptions _options;

        public UrlResolver(
            ISanityClient client,
            IOptions<SanityOptions> options,
            IHttpContextAccessor httpContextAccessor) : base(client)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;
        }

        public override async Task<string> GetUrl(ISanityDocument document, CultureInfo language)
        {
            if (document == null)
            {
                return null;
            }

            return await GetUrl(document.Id, language);
        }

        public override async Task<string> GetUrl(Guid documentId, CultureInfo language)
        {
            var request = _httpContextAccessor.HttpContext?.Request ?? new DefaultHttpContext().Request;
            var url = new StringBuilder();
            var usedValues = new HashSet<string>();
            var routeValues = new RouteValueDictionary(request.RouteValues)
            {
                [RoutingConstants.LanguageRouteValueKey] = language,
                [RoutingConstants.DocumentRouteValueKey] = documentId
            };

            if (!await AddVirtualPathFromSegments(url, request, routeValues, usedValues))
            {
                return null;
            }

            return url.ToString();
        }

        protected virtual async Task<bool> AddVirtualPathFromSegments(
            StringBuilder virtualPath,
            HttpRequest request,
            RouteValueDictionary routeValues,
            HashSet<string> usedValues)
        {
            for (var index = _options.RoutingSegments.Count - 1; index >= 0; index--)
            {
                var segment = _options.RoutingSegments[index];
                var segmentPath = await segment.GetVirtualPathSegment(request, routeValues, usedValues);

                if (segmentPath == null)
                {
                    return false;
                }

                if (segmentPath.Length > 0 && segment.AppendTrailingSlash)
                {
                    segmentPath = $"{segmentPath}/";
                }

                virtualPath.Insert(0, segmentPath);
            }

            virtualPath.Insert(0, '/');
            return true;
        }
    }
}