using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SanityDotNet.Client;
using SanityDotNet.Models;

namespace SanityDotNet.Mvc.Routing.Segments
{
    public class DocumentSegment : IUrlSegment
    {
        private readonly ISanityClient _sanityClient;

        public DocumentSegment(ISanityClient sanityClient)
        {
            _sanityClient = sanityClient;
        }

        public string Name => "document";

        public bool AppendTrailingSlash => false;

        public virtual async Task<bool> IsMatch(SegmentContext segmentContext)
        {
            while (true)
            {
                var segment = segmentContext.GetNextValue(segmentContext.RemainingPath);
                var segmentValue = segment.Next;
                var document = await _sanityClient.GetBySlug<ISanityDocument>(segmentValue, segmentContext.Language);

                if (document == null)
                {
                    var isValidRoute = segmentContext.RoutedDocument != null
                                       && segmentContext.RemainingPath.IndexOf('/') == -1;

                    if (!isValidRoute)
                    {
                        segmentContext.RoutedDocumentId = Guid.Empty;
                        segmentContext.RoutedDocument = null;
                    }

                    return isValidRoute;
                }

                if (!segmentContext.ConsumedDocuments.Contains(document.Id))
                {
                    segmentContext.ConsumedDocuments.Add(document.Id);
                }

                if (!IsValidParent(segmentContext, document))
                {
                    segmentContext.RoutedDocument = null;
                    return false;
                }

                segmentContext.RoutedDocument = document;
                segmentContext.RoutedDocumentId = document.Id;
                segmentContext.RemainingPath = segment.Remaining;

                if (!string.IsNullOrEmpty(segmentContext.RemainingPath))
                {
                    continue;
                }

                return true;
            }
        }

        public virtual async Task<string> GetVirtualPathSegment(
            HttpRequest request,
            RouteValueDictionary routeValues,
            HashSet<string> usedValues)
        {
            usedValues.Add(Name);

            if (routeValues.TryGetCustomValue<Guid>(RoutingConstants.DocumentRouteValueKey, out var documentId))
            {
                if (!routeValues.TryGetCustomValue<CultureInfo>(RoutingConstants.LanguageRouteValueKey, out var language))
                {
                    language = CultureInfo.CurrentUICulture;
                }

                var virtualPathBuilder = new StringBuilder();
                if (await AppendVirtualPath(virtualPathBuilder, documentId, language))
                {
                    return virtualPathBuilder.ToString();
                }
            }

            return null;
        }

        protected virtual async Task<bool> AppendVirtualPath(
            StringBuilder virtualPathBuilder,
            Guid documentId,
            CultureInfo language)
        {
            var document = await _sanityClient.Get<ISanityDocument>(documentId, language);

            if (document == null || !document.Slug.ContainsKey(language.TwoLetterISOLanguageName))
            {
                return false;
            }

            var slug = document.Slug[language.TwoLetterISOLanguageName];
            var parentRef = document.Parent;
            virtualPathBuilder.Insert(0, $"{slug}/");

            if (parentRef != null)
            {
                return await AppendVirtualPath(virtualPathBuilder, parentRef.Id, language);
            }

            return true;
        }

        private bool IsValidParent(SegmentContext segmentContext, ISanityDocument document)
        {
            if (document.Parent == null)
            {
                return true;
            }

            var indexOfParent = segmentContext.ConsumedDocuments.IndexOf(document.Parent.Id);
            return indexOfParent > -1 && indexOfParent == segmentContext.ConsumedDocuments.Count - 2;
        }
    }
}