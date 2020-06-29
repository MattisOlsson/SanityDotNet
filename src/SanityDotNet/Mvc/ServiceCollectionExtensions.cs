using System;
using System.Collections.Generic;
using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using SanityDotNet.Caching;
using SanityDotNet.Client;
using SanityDotNet.Models.Converters;
using SanityDotNet.Mvc.Globalization;
using SanityDotNet.Mvc.Reflection;
using SanityDotNet.Mvc.Routing;
using SanityDotNet.Mvc.Routing.Segments;
using SanityDotNet.QueryApi;
using SanityDotNet.Web;

namespace SanityDotNet.Mvc
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSanityMvc(this IServiceCollection services)
        {
            // IHttpContextAccessor
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Request localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("no");
                options.SupportedCultures = new List<CultureInfo>
                {
                    CultureInfo.GetCultureInfo("no"),
                    CultureInfo.GetCultureInfo("sv"),
                    CultureInfo.GetCultureInfo("en")
                };
                options.SupportedUICultures = options.SupportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteRequestCultureProvider(options.DefaultRequestCulture));
            });

            // MediatR
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            // Content services
            services.AddSingleton<ISanityTypeScanner, SanityTypeScanner>();
            services.AddSingleton<ISanityTemplateResolver, SanityTemplateResolver>();

            services.AddSingleton(provider => RestService.For<IQueryApi>("https://1k3uv2iq.api.sanity.io/v1",
                new RefitSettings
                {
                    ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            Converters = new List<JsonConverter>
                                {new SanityDocumentConverter(provider.GetRequiredService<ISanityTypeScanner>())}
                        }
                    )
                }));

            services.AddSingleton<ISanityCacheHandler, SanityCacheHandler>();
            services.AddSingleton(
                provider => new SanityClient(provider.GetRequiredService<IQueryApi>(), "production"));
            services.AddSingleton<ISanityClient, SanityClientCacheDecorator>(provider =>
                new SanityClientCacheDecorator(
                    provider.GetRequiredService<SanityClient>(),
                    provider.GetRequiredService<ISanityCacheHandler>()));

            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<SanityOptions>, SanityCoreOptionsSetup>());

            services.AddScoped<SanityRouteValueTransformer, SanityRouteValueTransformer>();
            services.AddScoped<IUrlResolver, UrlResolver>();
            services.AddSingleton(provider =>
                new SanityDocumentConverter(provider.GetRequiredService<ISanityTypeScanner>()));

            // Model binding
            services.AddSingleton<ComplexTypeModelBinderProvider>();
            services.AddMvc(options =>
            {
                options.ValueProviderFactories.Insert(0, new SanityDocumentValueProviderFactory());
                options.ModelBinderProviders.Insert(0, new SanityDocumentModelBinderProvider());
            });
        }

        public static void AddSanityMvc(this IServiceCollection services, Action<SanityOptions> santityOptions)
        {
            services.AddSanityMvc();
            services.Configure(santityOptions);
        }
    }
}