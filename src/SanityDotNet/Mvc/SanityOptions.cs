using System;
using System.Collections.Generic;
using SanityDotNet.Mvc.Routing.Segments;

namespace SanityDotNet.Mvc
{
    public class SanityOptions
    {
        public IList<IUrlSegment> RoutingSegments { get; set; }
        public IServiceProvider ServiceProvider { get; internal set; }
    }
}