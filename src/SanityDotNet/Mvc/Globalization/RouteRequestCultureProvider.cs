using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace SanityDotNet.Mvc.Globalization
{
    public class RouteRequestCultureProvider : IRequestCultureProvider
    {
        private readonly CultureInfo _defaultCulture;
        private readonly CultureInfo _defaultUiCulture;

        public RouteRequestCultureProvider(RequestCulture requestCulture)
        {
            _defaultCulture = requestCulture.Culture;
            _defaultUiCulture = requestCulture.UICulture;
        }

        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            //Parsing language from url path, which looks like "/en/home/index"
            PathString url = httpContext.Request.Path;

            // Test any culture in route
            if (url.ToString().Length <= 1)
            {
                CultureInfo.CurrentCulture = _defaultCulture;
                CultureInfo.CurrentUICulture = _defaultUiCulture;
                // Set default Culture and default UICulture
                return Task.FromResult(new ProviderCultureResult(_defaultCulture.Name, _defaultUiCulture.Name));
            }

            var parts = httpContext.Request.Path.Value.Split('/');
            var culture = parts[1];

            // Test if the culture is properly formatted
            if (!Regex.IsMatch(culture, @"^[a-z]{2}(-[A-Z]{2})*$"))
            {
                // Set default Culture and default UICulture
                CultureInfo.CurrentCulture = _defaultCulture;
                CultureInfo.CurrentUICulture = _defaultUiCulture;
                return Task.FromResult(new ProviderCultureResult(_defaultCulture.Name, _defaultUiCulture.Name));
            }

            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            // Set Culture and UICulture from route culture parameter
            return Task.FromResult(new ProviderCultureResult(culture, culture));
        }
    }}