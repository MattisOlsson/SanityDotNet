using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SanityDotNet.Expressions;

namespace SanityDotNet.Extensions
{
    public static class ExpressionExtensions
    {
        internal static object CompileInvoke(this Expression expression)
        {
            var l = Expression.Lambda(Expression.Convert(expression, expression.Type));
            return l.Compile().DynamicInvoke();
        }

        internal static T CompileInvoke<T>(this Expression expression)
        {
            var l = Expression.Lambda(Expression.Convert(expression, expression.Type));
            return (T) l.Compile().DynamicInvoke();
        }

        public static IEnumerable<T> Find<T>(this Expression expression, Func<T, bool> expressionToFind) where T : Expression
        {
            return new ExpressionFinder<T>().Find(expression, expressionToFind);
        }

        public static Expression Replace<T>(
            this Expression expression,
            Func<T, bool> predicate,
            Func<T, Expression> replaceWith) where T : Expression
        {
            return new ExpressionReplacer<T>().Replace(expression, predicate, replaceWith);
        }

        public static bool IsGetItemInvokationOnGenericDictionary(this MethodCallExpression methodCall)
        {
            if (methodCall?.Object == null)
            {
                return false;
            }

            var type = methodCall.Object.Type;
            if (!type.IsGenericType)
            {
                return false;
            }

            var genericArguments = type.GetGenericArguments();
            return genericArguments.Length == 2
                   && typeof(IDictionary<,>).MakeGenericType(genericArguments).IsAssignableFrom(type)
                   && methodCall.Method.Name == "get_Item";
        }
    }
}