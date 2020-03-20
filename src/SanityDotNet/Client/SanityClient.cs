using SanityDotNet.Extensions;
using SanityDotNet.Models;
using SanityDotNet.QueryApi;

namespace SanityDotNet.Client
{
    public class SanityClient : ISanityClient
    {
        public IQueryBuilder<T> Query<T>() where T : SanityDocument
        {
            var query = QueryBuilder<T>.Create();
            return query.Filter(m => m.Type.Is(typeof(T).Name.ToCamelCase()));
        }
    }
}