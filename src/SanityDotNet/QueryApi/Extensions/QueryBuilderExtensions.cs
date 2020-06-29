using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SanityDotNet.Models;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Projections;
using SanityDotNet.QueryApi.Sorting;

namespace SanityDotNet.QueryApi.Extensions
{
    public static class QueryBuilderExtensions
    {
        public static IQueryBuilder<TSource> Filter<TSource>(
            this IQueryBuilder<TSource> queryBuilder,
            Expression<Func<TSource, Filter>> filterExpression) where TSource : ISanityDocument
        {
            var filter = FilterExpression<TSource, Filter>.ParseFilterExpression(filterExpression);
            return queryBuilder.Filter(filter);
        }

        public static IQueryBuilder<TSource> OrFilter<TSource>(
            this IQueryBuilder<TSource> queryBuilder,
            Expression<Func<TSource, Filter>> filterExpression) where TSource : ISanityDocument
        {
            var filter = FilterExpression<TSource, Filter>.ParseFilterExpression(filterExpression);
            return queryBuilder.OrFilter(filter);
        }

        public static IQueryBuilder<TSource> Filter<TSource>(
            this IQueryBuilder<TSource> search,
            Filter filter) where TSource : ISanityDocument
        {
            return new QueryBuilder<TSource>(search,
                context => search.ApplyFilter(context, filter,
                    (existingFilter, newFilter) => existingFilter & newFilter));
        }

        public static IQueryBuilder<TSource> OrFilter<TSource>(
            this IQueryBuilder<TSource> search,
            Filter filter) where TSource : ISanityDocument
        {
            return new QueryBuilder<TSource>(search,
                context => search.ApplyFilter(context, filter,
                    (existingFilter, newFilter) => existingFilter | newFilter));
        }

        public static IQueryBuilder<TSource> OrderBy<TSource>(
            this IQueryBuilder<TSource> search,
            params Expression<Func<TSource, Sort>>[] expressions) where TSource : ISanityDocument
        {
            IQueryBuilder<TSource> newSearch = search;

            foreach (var expression in expressions)
            {
                var search1 = newSearch;
                var sort = FilterExpression<TSource, Sort>.ParseFilterExpression(expression);
                newSearch = new QueryBuilder<TSource>(search1,
                    context => search1.ApplySorting(context, sort));
            }

            return newSearch;
        }

        public static IQueryBuilder<TProjection> Project<TSource, TProjection>(
            this IQueryBuilder<TSource> search,
            params Expression<Func<TSource, Projection>>[] expressions) where TSource : ISanityDocument
        {
            return new QueryBuilder<TProjection>(search, context => search.ApplyProjections(context, expressions));
        }

        public static async Task<QueryResponse<T>> GetQueryResponse<T>(this IQueryBuilder<T> query)
        {
            return await query.Client.GetQueryResponse<T>(query);
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

        internal static void ApplySorting<T>(
            this IQueryBuilder<T> search,
            IQueryContext context,
            Sort sort)
        {
            if (sort == null)
            {
                return;
            }

            context.Query.Order.Add(sort);
        }

        internal static void ApplyProjections<T>(
            this IQueryBuilder<T> search,
            IQueryContext context,
            IEnumerable<Expression<Func<T, Projection>>> expressions)
        {
            foreach (var expression in expressions)
            {
                var projection = FilterExpression<T, Projection>.ParseFilterExpression(expression);
                context.Query.Projections.Add(projection);
            }
        }
    }
}