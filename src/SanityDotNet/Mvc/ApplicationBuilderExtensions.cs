using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SanityDotNet.Mvc.Reflection;

namespace SanityDotNet.Mvc
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSanity(this IApplicationBuilder app)
        {
            app.UseRequestLocalization();
            app.ApplicationServices.GetRequiredService<ISanityTypeScanner>().Scan();
            return app;
        }
    }
}