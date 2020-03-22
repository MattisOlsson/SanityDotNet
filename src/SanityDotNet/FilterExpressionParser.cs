using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SanityDotNet.Extensions;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.Reflection;

namespace SanityDotNet
{
    public class FilterExpressionParser<TFilter> where TFilter : Filter
    {
        public virtual TFilter GetFilter<TSource>(
            Expression<Func<TSource, TFilter>> filterExpression)
        {
            Validate(filterExpression);
            var expression = filterExpression.Body;

            while (expression.Find(new Func<MethodCallExpression, bool>(ReturnsFilterExpression)).Any())
            {
                expression = expression.Replace(
                    ReturnsFilterExpression,
                    new Func<MethodCallExpression, Expression>(RealizeFilterExpressionMethodCalls));
            }

            return expression.Replace(
                    x => typeof(IDelegateFilter<TFilter>).IsAssignableFrom(x.Method.ReturnType),
                    (Func<MethodCallExpression, Expression>) (x =>
                        (Expression) Expression.Constant(GetFilterFromDelegateFilterMethod(x, null))))
                .CompileInvoke<TFilter>();
        }

        protected virtual void Validate<TSource>(
            Expression<Func<TSource, TFilter>> filterExpression)
        {
            var delegateType = typeof(IDelegateFilter<TFilter>);
            if (filterExpression.Body
                .Find<NewExpression>(x => delegateType.IsAssignableFrom(x.Type)).Any())
            {
                throw new NotSupportedException(
                    $"Instantiating new {delegateType.Name} is not supported.The {delegateType.Name} class is intended to be used as a return value from extensions methods.");
            }

            var source1 = filterExpression.Body.Find(
                (Func<MethodCallExpression, bool>) (x =>
                    !x.Method.IsExtensionMethod() && delegateType.IsAssignableFrom(x.Method.ReturnType))).ToList();
            if (source1.Any())
            {
                throw new NotSupportedException(
                    $"Method {source1.First().Method.Name} returns {delegateType.Name}. Only extension methods should return {delegateType.Name}.");
            }

            if (filterExpression.Body.Find((Func<NewExpression, bool>) (x =>
                x.Type.IsGenericType && x.Type.GetGenericTypeDefinition() == typeof(FilterExpression<,>))).Any())
            {
                throw new NotSupportedException(
                    $"Instantiating new {typeof(FilterExpression<,>).Name} is not supported.The {typeof(FilterExpression<,>).Name} class is intended to be used as a return value from extensions methods.");
            }

            var source2 = filterExpression.Body.Find(
                (Func<MethodCallExpression, bool>) (x =>
                    !x.Method.IsExtensionMethod() && x.Type.IsGenericType &&
                    x.Type.GetGenericTypeDefinition() == typeof(FilterExpression<,>))).ToList();

            if (source2.Any())
            {
                throw new NotSupportedException(
                    $"Method {source2.First().Method.Name} returns {typeof(FilterExpression<,>).Name}. Only extension methods should return {typeof(FilterExpression<,>).Name}.");
            }
        }

        protected bool ReturnsFilterExpression(MethodCallExpression x)
        {
            return x.Method.HasGenericTypeDefinition(typeof(FilterExpression<,>));
        }

        protected Expression RealizeFilterExpressionMethodCalls(
            MethodCallExpression methodCall)
        {
            return GetExpressionFromFilterExpressionMethod(methodCall);
        }

        protected Expression GetExpressionFromFilterExpressionMethod(
            MethodCallExpression methodExpression)
        {
            var methodArguments = new List<object>
            {
                null
            };

            for (var index = 1; index < methodExpression.Arguments.Count; ++index)
            {
                methodArguments.Add(methodExpression.Arguments[index].CompileInvoke());
            }

            var obj1 = methodExpression.Method.Invoke(null, methodArguments.ToArray());
            var obj2 = typeof(FilterExpression<,>)
                .MakeGenericType(methodExpression.Method.ReturnType.GetGenericArguments()[0]).GetProperty("Expression")
                ?.GetGetMethod()
                .Invoke(obj1, new object[0]);

            var expression =
                (Expression) typeof(Expression<>).GetProperty("Body")?.GetGetMethod().Invoke(obj2, new object[0]);
            var invocationTarget = ((LambdaExpression) obj2)?.Parameters[0];
            var actualInvocationTarget = methodExpression.Arguments[0];

            bool Predicate(Expression x) => x == invocationTarget;
            Expression ReplaceWith(Expression x) => actualInvocationTarget;

            return expression.Replace(Predicate,
                (Func<Expression, Expression>) ReplaceWith);
        }

        protected TFilter GetFilterFromDelegateFilterMethod(
            MethodCallExpression methodExpression,
            string fieldName)
        {
            if (fieldName == null)
            {
                fieldName = string.Empty;
            }

            var methodArguments = new List<object>
            {
                null
            };

            for (var index = 1; index < methodExpression.Arguments.Count; ++index)
            {
                methodArguments.Add(methodExpression.Arguments[index].CompileInvoke());
            }

            var delegateFilterBuilder =
                (IDelegateFilter<TFilter>) methodExpression.Method.Invoke(null, methodArguments.ToArray());

            if (!IsFieldNameExpression(methodExpression.Arguments[0]))
            {
                return delegateFilterBuilder.GetFilter(fieldName);
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                fieldName += ".";
            }

            fieldName += methodExpression.Arguments[0].GetFieldPath();

            return delegateFilterBuilder.GetFilter(fieldName);
        }

        private static bool IsFieldNameExpression(Expression expression)
        {
            switch (expression)
            {
                case MemberExpression _:
                case MethodCallExpression _:
                    return true;
                default:
                    var returnType = expression.GetReturnType();
                    return typeof(Enum).IsAssignableFrom(returnType) || returnType.IsGenericType &&
                        returnType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                        typeof(Enum).IsAssignableFrom(returnType.GetGenericArguments()[0]);
            }
        }
    }
}