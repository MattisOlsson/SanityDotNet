using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SanityDotNet.Controllers;
using SanityDotNet.Models;
using SanityDotNet.QueryApi;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Sorting;
using SanityDotNet.Reflection;

namespace SanityDotNet.Extensions
{
    public static class QueryBuilderExtensions
    {
        public static IQueryBuilder<TSource> Filter<TSource>(
            this IQueryBuilder<TSource> queryBuilder,
            Expression<Func<TSource, Filter>> filterExpression) where TSource : SanityDocument
        {
            var filter = FilterExpression<TSource, Filter>.ParseFilterExpression(queryBuilder, filterExpression);
            return queryBuilder.Filter(filter);
        }

        public static IQueryBuilder<TSource> OrFilter<TSource>(
            this IQueryBuilder<TSource> queryBuilder,
            Expression<Func<TSource, Filter>> filterExpression) where TSource : SanityDocument
        {
            var filter = FilterExpression<TSource, Filter>.ParseFilterExpression(queryBuilder, filterExpression);
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

        public static IQueryBuilder<TSource> OrderBy<TSource>(
            this IQueryBuilder<TSource> search,
            params Expression<Func<TSource, Sort>>[] expressions) where TSource : SanityDocument
        {
            IQueryBuilder<TSource> newSearch = search;

            foreach (var expression in expressions)
            {
                var search1 = newSearch;
                var sort = FilterExpression<TSource, Sort>.ParseFilterExpression(search1, expression);
                newSearch = new QueryBuilder<TSource>(search1,
                    context => search1.ApplySorting(context, sort));
            }

            return newSearch;
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

        public static QueryResponse<T> GetQueryResult<T>(this IQueryBuilder<T> query)
        {
            var context = new QueryContext
            {
                Query = query
            };

            query.ApplyActions(context);
            return new QueryResponse<T>
            {
                Duration = 1,
                Query = context.Query.ToQueryString(),
                Result = Enumerable.Empty<T>()
            };
        }

        public static string GetQueryString<T>(this IQueryBuilder<T> query)
        {
            var context = new QueryContext
            {
                Query = query
            };

            query.ApplyActions(context);
            return context.Query.ToQueryString();
        }
    }
}