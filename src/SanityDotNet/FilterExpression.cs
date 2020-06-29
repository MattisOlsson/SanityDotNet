using System;
using System.Linq.Expressions;
using SanityDotNet.QueryApi;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet
{
    public class FilterExpression<T, TFilter> : Filter where TFilter : Filter
    {
        internal static TFilter ParseFilterExpression<TSource>(
            Expression<Func<TSource, TFilter>> filterExpression)
        {
            return new FilterExpressionParser<TFilter>().GetFilter(filterExpression);
        }

        public FilterExpression(Expression<Func<T, TFilter>> expression)
        {
            Expression = expression;
        }

        public Expression<Func<T, TFilter>> Expression { get; set; }
    }

}