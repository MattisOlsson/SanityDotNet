using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SanityDotNet.Extensions;
using SanityDotNet.Models;
using SanityDotNet.QueryApi;
using SanityDotNet.QueryApi.Extensions;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet.Client
{
    public class SanityClient : ISanityClient
    {
        private readonly IQueryApi _api;
        private readonly string _dataset;

        public SanityClient(IQueryApi api, string dataset)
        {
            _api = api;
            _dataset = dataset;
        }

        public virtual IQueryBuilder<T> Query<T>() where T : ISanityDocument
        {
            return Query<T>(CultureInfo.CurrentUICulture);
        }

        public virtual IQueryBuilder<T> Query<T>(CultureInfo language) where T : ISanityDocument
        {
            var query = new QueryBuilder<T> {Client = this, Language = language};

            return typeof(T) == typeof(ISanityDocument)
                ? query
                : query.Filter(m => m.Type.Is(typeof(T).Name.ToCamelCase()));
        }

        public virtual IQueryBuilder<ISanityDocument> Query(Type contentType, CultureInfo language)
        {
            var query = new QueryBuilder<ISanityDocument> {Client = this, Language = language};
            return query.Filter(m => m.Type.Is(contentType.Name.ToCamelCase()));
        }

        public virtual IQueryBuilder<ISanityDocument> Query(CultureInfo language)
        {
            return Query<ISanityDocument>(language);
        }

        public virtual IQueryBuilder<ISanityDocument> Query()
        {
            return Query<ISanityDocument>(CultureInfo.CurrentUICulture);
        }

        public async Task<T> Get<T>(Guid documentId, CultureInfo language) where T : ISanityDocument
        {
            var query = Query<T>(language).Filter(x => x.Id.Is(documentId));
            var response = await GetQueryResponse<T>(query);

            if (response.Result.Any())
            {
                return response.Result.First();
            }

            return default;
        }

        public virtual async Task<T> GetBySlug<T>(string slug, CultureInfo language) where T : ISanityDocument
        {
            var query = Query<T>(language).Filter(doc => doc.Slug.Is(slug));
            var response = await GetQueryResponse<T>(query);

            return response.Result.Any()
                ? response.Result.First()
                : default;
        }

        public async Task<QueryResponse<T>> GetQueryResponse<T>(IQueryBuilder query)
        {
            using var context = new QueryContext(query);
            query.ApplyActions(context);
            return await _api.Query<T>(_dataset, query.ToQueryString());
        }
    }
}