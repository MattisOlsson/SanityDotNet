using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SanityDotNet.Models;

namespace SanityDotNet.Caching
{
    public interface ISanityCacheHandler
    {
        Task<T> GetCachedOrLoad<T>(Guid documentId, CultureInfo language, Func<Task<T>> loadAction) where T : ISanityDocument;
        Task<T> GetCachedOrLoad<T>(string slug, CultureInfo language, Func<Task<T>> loadAction) where T : ISanityDocument;
        void Invalidate(Guid documentId);
        void Invalidate(IEnumerable<Guid> documentIds);
    }
}