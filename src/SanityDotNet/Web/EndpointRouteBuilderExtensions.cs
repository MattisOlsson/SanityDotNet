using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using SanityDotNet.Mvc.Routing;

namespace SanityDotNet.Web
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapSanityRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute("Home", "{controller=StartPage}/{action=index}");
            endpoints.MapControllerRoute("Webhooks", "webhooks/{controller}/{action=index}/{id?}");
            endpoints.MapDynamicControllerRoute<SanityRouteValueTransformer>("{**slugs}");
            return endpoints;
        }
    }
}