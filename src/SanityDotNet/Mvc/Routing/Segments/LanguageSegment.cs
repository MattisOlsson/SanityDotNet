using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace SanityDotNet.Mvc.Routing.Segments
{
    public class LanguageSegment : IUrlSegment
    {
        public string Name => "language";

        public bool AppendTrailingSlash => true;

        public Task<bool> IsMatch(SegmentContext segmentContext)
        {
            var next = segmentContext.GetNextValue(segmentContext.RemainingPath);

            if (CultureInfo.CurrentUICulture.Name.Equals(next.Next, StringComparison.CurrentCultureIgnoreCase))
            {
                segmentContext.RemainingPath = next.Remaining;
            }

            return Task.FromResult(true);
        }

        public Task<string> GetVirtualPathSegment(
            HttpRequest request,
            RouteValueDictionary routeValues,
            HashSet<string> usedValues)
        {
            return Task.FromResult(string.Empty);
        }
    }
}