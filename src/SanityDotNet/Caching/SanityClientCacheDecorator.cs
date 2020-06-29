using System;
using System.Globalization;
using System.Threading.Tasks;
using SanityDotNet.Client;
using SanityDotNet.Models;
using SanityDotNet.QueryApi;

namespace SanityDotNet.Caching
{
    public class SanityClientCacheDecorator : ISanityClient
    {
        private readonly ISanityClient _inner;
        private readonly ISanityCacheHandler _cacheHandler;

        public SanityClientCacheDecorator(ISanityClient inner, ISanityCacheHandler cacheHandler)
        {
            _inner = inner;
            _cacheHandler = cacheHandler;
        }

        public IQueryBuilder<T> Query<T>() where T : ISanityDocument
        {
            return _inner.Query<T>();
        }

        public IQueryBuilder<T> Query<T>(CultureInfo language) where T : ISanityDocument
        {
            return _inner.Query<T>(language);
        }

        public IQueryBuilder<ISanityDocument> Query(Type contentType, CultureInfo language)
        {
            return _inner.Query(contentType, language);
        }

        public IQueryBuilder<ISanityDocument> Query(CultureInfo language)
        {
            return _inner.Query(language);
        }

        public IQueryBuilder<ISanityDocument> Query()
        {
            return _inner.Query<ISanityDocument>();
        }

        public async Task<T> Get<T>(Guid documentId, CultureInfo language) where T : ISanityDocument
        {
            return await _cacheHandler.GetCachedOrLoad(documentId, language, () => _inner.Get<T>(documentId, language));
        }

        public async Task<T> GetBySlug<T>(string slug, CultureInfo language) where T : ISanityDocument
        {
            return await _cacheHandler.GetCachedOrLoad(slug, language, () => _inner.GetBySlug<T>(slug, language));
        }

        public async Task<QueryResponse<T>> GetQueryResponse<T>(IQueryBuilder query)
        {
            return await _inner.GetQueryResponse<T>(query);
        }
    }
}