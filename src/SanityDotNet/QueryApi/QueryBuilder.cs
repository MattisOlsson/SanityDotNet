using System;
using System.Text;
using SanityDotNet.Controllers;
using SanityDotNet.Extensions;
using SanityDotNet.Models;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Sorting;

namespace SanityDotNet.QueryApi
{
    public class QueryBuilder<T> : IQueryBuilder<T> where T : SanityDocument
    {
        private readonly Action<IQueryContext> _action;
        private readonly IQueryBuilder<T> _wrapped;

        public static IQueryBuilder<T> Create()
        {
            return new QueryBuilder<T>();
        }

        public QueryBuilder()
        {
            Order = new SortList();
        }

        public QueryBuilder(IQueryBuilder<T> queryBuilder, Action<IQueryContext> action) : this()
        {
            _wrapped = queryBuilder;
            _action = action;
        }

        public Filter Filter { get; set; }
        public SortList Order { get; set; }

        public void ApplyActions(IQueryContext context)
        {
            _wrapped?.ApplyActions(context);
            _action?.Invoke(context);
        }

        public string ToQueryString()
        {
            var sorting = GetSortingString();

            return !string.IsNullOrEmpty(sorting)
                ? $"*[{ToString()}] | {sorting}"
                : $"*[{ToString()}]";
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
    }
}