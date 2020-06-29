using System;
using System.Globalization;
using System.Threading.Tasks;
using SanityDotNet.Models;
using SanityDotNet.QueryApi;

namespace SanityDotNet.Client
{
    public interface ISanityClient
    {
        IQueryBuilder<T> Query<T>() where T : ISanityDocument;
        IQueryBuilder<T> Query<T>(CultureInfo language) where T : ISanityDocument;
        IQueryBuilder<ISanityDocument> Query(Type contentType, CultureInfo language);
        IQueryBuilder<ISanityDocument> Query(CultureInfo language);
        IQueryBuilder<ISanityDocument> Query();
        Task<T> Get<T>(Guid documentId, CultureInfo language) where T : ISanityDocument;
        Task<T> GetBySlug<T>(string slug, CultureInfo language) where T : ISanityDocument;
        Task<QueryResponse<T>> GetQueryResponse<T>(IQueryBuilder query);
    }
}