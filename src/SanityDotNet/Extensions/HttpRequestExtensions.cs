using System;
using Microsoft.AspNetCore.Http;

namespace SanityDotNet.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri ToAbsoluteUri(this HttpRequest request)
        {
            var builder = new UriBuilder {Scheme = request.Scheme, Host = request.Host.Host};

            if (request.Host.Port.HasValue)
                builder.Port = request.Host.Port.Value;

            builder.Path = request.Path;
            builder.Query = request.QueryString.ToString();
            return builder.Uri;
        }
    }
}