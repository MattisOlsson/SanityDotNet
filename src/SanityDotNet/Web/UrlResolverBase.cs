using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SanityDotNet.Client;
using SanityDotNet.Extensions;
using SanityDotNet.Models;
using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.Web
{
    public abstract class UrlResolverBase : IUrlResolver
    {
        protected readonly ISanityClient Client;

        protected UrlResolverBase(ISanityClient client)
        {
            Client = client;
        }

        public virtual async Task<ISanityDocument> GetDocument(Guid documentId, CultureInfo language)
        {
            return await GetDocument<ISanityDocument>(new Reference {Id = documentId}, language);
        }

        public virtual async Task<TDoc> GetDocument<TDoc>(Reference documentReference, CultureInfo language) where TDoc : ISanityDocument
        {
            if (documentReference == null)
            {
                return default;
            }

            return await Client.Get<TDoc>(documentReference.Id, language);
        }

        public virtual async Task<string> GetUrl(ISanityDocument document)
        {
            return await GetUrl(document, CultureInfo.CurrentUICulture);
        }

        public virtual async Task<string> GetUrl(Guid documentId)
        {
            return await GetUrl(documentId, CultureInfo.CurrentUICulture);
        }

        public abstract Task<string> GetUrl(ISanityDocument document, CultureInfo language);
        public abstract Task<string> GetUrl(Guid documentId, CultureInfo language);
    }
}