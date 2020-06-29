using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SanityDotNet.Models;
using SanityDotNet.Web;

namespace SanityDotNet.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string DocumentUrl(this IUrlHelper url, Guid documentId)
        {
            return url.DocumentUrl(documentId, CultureInfo.CurrentUICulture);
        }

        public static string DocumentUrl(this IUrlHelper url, Guid documentId, CultureInfo language)
        {
            var urlResolver = url.ActionContext.HttpContext.RequestServices.GetRequiredService<IUrlResolver>();
            return urlResolver
                .GetUrl(documentId, language)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public static async Task<string> DocumentUrl(this IUrlHelper url, ISanityDocument document)
        {
            return await url.DocumentUrl(document, CultureInfo.CurrentUICulture);
        }

        public static async Task<string> DocumentUrl(this IUrlHelper url, ISanityDocument document, CultureInfo language)
        {
            var urlResolver = url.ActionContext.HttpContext.RequestServices.GetRequiredService<IUrlResolver>();
            return await urlResolver
                .GetUrl(document, language);
        }
    }
}