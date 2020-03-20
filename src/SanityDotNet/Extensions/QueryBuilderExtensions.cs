using System;
using System.Linq.Expressions;
using SanityDotNet.Controllers;
using SanityDotNet.Models;
using SanityDotNet.QueryApi;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet.Extensions
{
    public static class QueryBuilderExtensions
    {
        public static IQueryBuilder<TSource> Filter<TSource>(
            this IQueryBuilder<TSource> queryBuilder,
            Expression<Func<TSource, Filter>> filterExpression) where TSource : SanityDocument
        {
            var filter = FilterExpression<TSource>.ParseFilterExpression(queryBuilder, filterExpression);
            return queryBuilder.Filter(filter);
        }

        public static IQueryBuilder<TSource> OrFilter<TSource>(
            this IQueryBuilder<TSource> queryBuilder,
            Expression<Func<TSource, Filter>> filterExpression) where TSource : SanityDocument
        {
            var filter = FilterExpression<TSource>.ParseFilterExpression(queryBuilder, filterExpression);
            return queryBuilder.OrFilter(filter);
        }

        public static IQueryBuilder<TSource> Filter<TSource>(
            this IQueryBuilder<TSource> search,
            Filter filter) where TSource : SanityDocument
        {
            return new QueryBuilder<TSource>(search,
                context => search.ApplyFilter(context, filter,
                    (existingFilter, newFilter) => existingFilter & newFilter));
        }

        public static IQueryBuilder<TSource> OrFilter<TSource>(
            this IQueryBuilder<TSource> search,
            Filter filter) where TSource : SanityDocument
        {
            return new QueryBuilder<TSource>(search,
                context => search.ApplyFilter(context, filter,
                    (existingFilter, newFilter) => existingFilter | newFilter));
        }

        internal static void ApplyFilter<T>(
            this IQueryBuilder<T> query,
            IQueryContext context,
            Filter filter,
            Func<Filter, Filter, Filter> combineWithExisting)
        {
            if (filter == null)
            {
                return;
            }

            var currentFilter = context.Query.Filter;

            context.Query.Filter = currentFilter != null
                ? combineWithExisting(currentFilter, filter)
                : filter;
        }

        public static T GetQueryResult<T>(this IQueryBuilder<T> query)
        {
            var context = new QueryContext
            {
                Query = query
            };

            query.ApplyActions(context);
            var queryString = context.Query.ToQueryString();
            return default(T);
        }
    }
}