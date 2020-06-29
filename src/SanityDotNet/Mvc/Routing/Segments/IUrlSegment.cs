using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace SanityDotNet.Mvc.Routing.Segments
{
    public interface IUrlSegment
    {
        string Name { get; }

        bool AppendTrailingSlash { get; }

        Task<bool> IsMatch(SegmentContext segmentContext);

        Task<string> GetVirtualPathSegment(
            HttpRequest request,
            RouteValueDictionary routeValues,
            HashSet<string> usedValues);
    }
}