using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SanityDotNet.Models;

namespace SanityDotNet.Caching
{
    public class SanityCacheHandler : ISanityCacheHandler
    {
        private readonly IMemoryCache _cache;

        public SanityCacheHandler(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetCachedOrLoad<T>(Guid documentId, CultureInfo language, Func<Task<T>> loadAction)
            where T : ISanityDocument
        {
            var cacheKey = CreateCacheKey(documentId);

            if (!_cache.TryGetValue(cacheKey, out T item))
            {
                item = await loadAction();
                AddToCache(cacheKey, item);

                if (item != null)
                {
                    AddToCache(CreateSlugMappingCacheKey(item.Slug.Current, language), item.Id);
                }
            }

            return item;
        }

        public async Task<T> GetCachedOrLoad<T>(string slug, CultureInfo language, Func<Task<T>> loadAction)
            where T : ISanityDocument
        {
            var slugDocumentIdCacheKey = CreateSlugMappingCacheKey(slug, language);

            if (!_cache.TryGetValue(slugDocumentIdCacheKey, out Guid documentId))
            {
                var item = await loadAction();
                AddToCache(slugDocumentIdCacheKey, item?.Id ?? Guid.Empty);

                if (item != null)
                {
                    var cacheKey = CreateCacheKey(item.Id);
                    AddToCache(cacheKey, item);
                }

                return item;
            }

            return await GetCachedOrLoad(documentId, language, loadAction);
        }

        public virtual void Invalidate(Guid documentId)
        {
            _cache.Remove(CreateCacheKey(documentId));
        }

        public virtual void Invalidate(IEnumerable<Guid> documentIds)
        {
            foreach (var documentId in documentIds)
            {
                Invalidate(documentId);
            }
        }

        protected virtual void AddToCache<T>(string cacheKey, T item)
        {
            _cache.Set(cacheKey, item, new MemoryCacheEntryOptions {SlidingExpiration = TimeSpan.FromHours(12)});
        }

        protected virtual string CreateCacheKey(Guid documentId)
        {
            return $"SanityCache:Document:{documentId}";
        }

        protected virtual string CreateSlugMappingCacheKey(string slug, CultureInfo language)
        {
            return $"SanityCache:Slug-{language.TwoLetterISOLanguageName}:{slug}";
        }
    }
}