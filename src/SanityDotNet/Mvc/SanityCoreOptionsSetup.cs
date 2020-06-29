using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SanityDotNet.Client;
using SanityDotNet.Mvc.Routing.Segments;

namespace SanityDotNet.Mvc
{
    public class SanityCoreOptionsSetup : IConfigureOptions<SanityOptions>
    {
        private readonly IServiceProvider _provider;

        public SanityCoreOptionsSetup(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SanityOptions options)
        {
            options.ServiceProvider = _provider;
            options.RoutingSegments = new List<IUrlSegment>
            {
                new LanguageSegment(),
                new DocumentSegment(_provider.GetRequiredService<ISanityClient>())
            };
        }
    }
}