using SanityDotNet.Controllers;
using SanityDotNet.Models;
using SanityDotNet.QueryApi;

namespace SanityDotNet.Client
{
    public interface ISanityClient
    {
        IQueryBuilder<T> Query<T>() where T : SanityDocument;
    }
}