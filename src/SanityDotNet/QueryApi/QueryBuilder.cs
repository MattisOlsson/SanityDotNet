using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using SanityDotNet.Client;
using SanityDotNet.Models;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Projections;
using SanityDotNet.QueryApi.Sorting;

namespace SanityDotNet.QueryApi
{
    public class QueryBuilder<T> : IQueryBuilder<T>
    {
        private readonly Action<IQueryContext> _action;
        private readonly IQueryBuilder _wrapped;

        public QueryBuilder()
        {
            Order = new SortList();
            Projections = new List<Projection>();
        }

        public QueryBuilder(IQueryBuilder queryBuilder, Action<IQueryContext> action) : this()
        {
            Client = queryBuilder.Client;
            Language = queryBuilder.Language;
            Projections = queryBuilder.Projections;
            _wrapped = queryBuilder;
            _action = action;
        }

        public List<Projection> Projections { get; set; }

        public ISanityClient Client { get; internal set; }

        public Filter Filter { get; set; }

        public SortList Order { get; set; }

        public CultureInfo Language { get; set; }

        public void ApplyActions(IQueryContext context)
        {
            _wrapped?.ApplyActions(context);
            _action?.Invoke(context);
        }

        public string ToQueryString()
        {
            var sorting = GetSortingString();

            return !string.IsNullOrEmpty(sorting)
                ? $"*[{ToString()}]|{sorting}{GetProjections()}"
                : $"*[{ToString()}]{GetProjections()}";
        }

        public string ToEncodedQueryString()
        {
            return HttpUtility.UrlEncode(ToQueryString());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (_wrapped != null)
            {
                sb.Append(_wrapped);
            }

            sb.Append(Filter?.ToString() ?? string.Empty);

            return sb.ToString();
        }

        public virtual string GetSortingString()
        {
            var sb = new StringBuilder();
            sb.Append(Order?.ToString() ?? string.Empty);
            return sb.ToString();
        }

        public virtual string GetProjections()
        {
            if (Projections.Count == 0)
            {
                return string.Empty;
            }

            return $"{{{string.Join(",", Projections.Select(p => p.ToString()))}}}";
        }
    }
}