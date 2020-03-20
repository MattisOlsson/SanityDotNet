using System;
using System.Linq.Expressions;
using SanityDotNet.QueryApi;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet
{
    public class FilterExpression<T> : Filter
    {
        internal static Filter ParseFilterExpression<TSource>(
            IQueryBuilder<TSource> search,
            Expression<Func<TSource, Filter>> filterExpression)
        {
            return new FilterExpressionParser().GetFilter(filterExpression);
        }

        public FilterExpression(Expression<Func<T, Filter>> expression)
        {
            Expression = expression;
        }

        public Expression<Func<T, Filter>> Expression { get; set; }
    }

}