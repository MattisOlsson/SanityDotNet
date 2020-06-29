using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SanityDotNet.Client;
using SanityDotNet.Mvc;
using SanityDotNet.Mvc.Routing.Segments;

namespace SanityDotNet.Web.Test
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSanityMvc(options =>
            {
                options.RoutingSegments = new List<IUrlSegment>
                {
                    new LanguageSegment(),
                    new DocumentSegment(options.ServiceProvider.GetRequiredService<ISanityClient>())
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSanity();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapSanityRoutes();
            });
        }
    }
}