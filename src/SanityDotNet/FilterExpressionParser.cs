using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SanityDotNet.Extensions;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.Reflection;

namespace SanityDotNet
{
    public class FilterExpressionParser
    {
        public virtual Filter GetFilter<TSource>(
            Expression<Func<TSource, Filter>> filterExpression)
        {
            ValidateFilterExpression(filterExpression);
            var expression = filterExpression.Body;

            while (expression.Find(new Func<MethodCallExpression, bool>(ReturnsFilterExpression)).Any())
            {
                expression = expression.Replace(
                    ReturnsFilterExpression,
                    new Func<MethodCallExpression, Expression>(RealizeFilterExpressionMethodCalls));
            }

            return expression.Replace(
                    x => x.Method.ReturnType == typeof(DelegateFilterBuilder),
                    (Func<MethodCallExpression, Expression>) (x =>
                        (Expression) Expression.Constant(GetFilterFromDelegateFilterBuilderMethod(x, null))))
                .CompileInvoke<Filter>();
        }

        protected virtual void ValidateFilterExpression<TSource>(
            Expression<Func<TSource, Filter>> filterExpression)
        {
            if (filterExpression.Body
                .Find<NewExpression>(x => x.Type == typeof(DelegateFilterBuilder)).Any())
            {
                throw new NotSupportedException(
                    $"Instantiating new {typeof(DelegateFilterBuilder).Name} is not supported.The {typeof(DelegateFilterBuilder).Name} class is intended to be used as a return value from extensions methods.");
            }

            var source1 = filterExpression.Body.Find(
                (Func<MethodCallExpression, bool>) (x =>
                    !x.Method.IsExtensionMethod() && x.Method.ReturnType == typeof(DelegateFilterBuilder))).ToList();
            if (source1.Any())
            {
                throw new NotSupportedException(
                    $"Method {source1.First().Method.Name} returns {typeof(DelegateFilterBuilder).Name}. Only extension methods should return {typeof(DelegateFilterBuilder).Name}.");
            }

            if (filterExpression.Body.Find((Func<NewExpression, bool>) (x =>
                x.Type.IsGenericType && x.Type.GetGenericTypeDefinition() == typeof(FilterExpression<>))).Any())
            {
                throw new NotSupportedException(
                    $"Instantiating new {typeof(FilterExpression<>).Name} is not supported.The {typeof(FilterExpression<>).Name} class is intended to be used as a return value from extensions methods.");
            }

            var source2 = filterExpression.Body.Find(
                (Func<MethodCallExpression, bool>) (x =>
                    !x.Method.IsExtensionMethod() && x.Type.IsGenericType &&
                    x.Type.GetGenericTypeDefinition() == typeof(FilterExpression<>))).ToList();

            if (source2.Any())
            {
                throw new NotSupportedException(
                    $"Method {source2.First().Method.Name} returns {typeof(FilterExpression<>).Name}. Only extension methods should return {typeof(FilterExpression<>).Name}.");
            }
        }

        protected bool ReturnsFilterExpression(MethodCallExpression x)
        {
            return x.Method.HasGenericTypeDefinition(typeof(FilterExpression<>));
        }

        protected Expression RealizeFilterExpressionMethodCalls(
            MethodCallExpression methodCall)
        {
            return GetExpressionFromFilterExpressionMethod(methodCall);
        }

        protected Expression GetExpressionFromFilterExpressionMethod(
            MethodCallExpression methodExpression)
        {
            var objectList = new List<object>
            {
                null
            };

            for (var index = 1; index < methodExpression.Arguments.Count; ++index)
            {
                objectList.Add(methodExpression.Arguments[index].CompileInvoke());
            }

            var obj1 = methodExpression.Method.Invoke(null, objectList.ToArray());
            var obj2 = typeof(FilterExpression<>)
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

        protected Filter GetFilterFromDelegateFilterBuilderMethod(
            MethodCallExpression methodExpression,
            string fieldName)
        {
            if (fieldName == null)
            {
                fieldName = string.Empty;
            }

            var objectList = new List<object>
            {
                null
            };

            for (var index = 1; index < methodExpression.Arguments.Count; ++index)
            {
                objectList.Add(methodExpression.Arguments[index].CompileInvoke());
            }

            var delegateFilterBuilder =
                (DelegateFilterBuilder) methodExpression.Method.Invoke(null, objectList.ToArray());

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