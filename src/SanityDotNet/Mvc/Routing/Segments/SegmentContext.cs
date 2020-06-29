using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SanityDotNet.Models;

namespace SanityDotNet.Mvc.Routing.Segments
{
    public class SegmentContext
    {
        private string _remainingPath;
        public readonly HttpRequest Request;
        public readonly List<Guid> ConsumedDocuments;

        public SegmentContext(HttpRequest request)
        {
            Request = request;
            ConsumedDocuments = new List<Guid>();
        }

        public string LastConsumedFragment { get; set; }

        public string RemainingPath
        {
            get => _remainingPath;
            set
            {
                LastConsumedFragment = _remainingPath != null ? GetNextValue(_remainingPath).Next : null;
                _remainingPath = value;
            }
        }

        public CultureInfo Language { get; set; }

        public Guid RoutedDocumentId { get; set; }

        public ISanityDocument RoutedDocument { get; set; }

        public SegmentPair GetNextValue(string remainingPath)
        {
            if (string.IsNullOrEmpty(remainingPath))
            {
                return new SegmentPair
                {
                    Next = string.Empty,
                    Remaining = string.Empty
                };
            }

            var length = remainingPath.IndexOf('/');
            string str;

            if (length != -1)
            {
                str = remainingPath.Substring(0, length);
                remainingPath = remainingPath.Substring(length + 1);
            }
            else
            {
                str = remainingPath;
                remainingPath = string.Empty;
            }

            return new SegmentPair
            {
                Next = str,
                Remaining = remainingPath
            };
        }
    }
}