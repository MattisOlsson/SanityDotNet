using System;
using System.Linq.Expressions;
using SanityDotNet.Expressions;

namespace SanityDotNet.Reflection
{
    public static class ExpressionExtensions
    {
        public static Type GetReturnType(this Expression expression)
        {
            if (expression is LambdaExpression lambdaExpression)
            {
                expression = lambdaExpression.Body;
            }

            Type type = null;
            switch (expression)
            {
                case MemberExpression _:
                    type = expression.Type;
                    break;
                case MethodCallExpression callExpression:
                    type = callExpression.Method.ReturnType;
                    break;
                case UnaryExpression unaryExpression:
                    type = unaryExpression.Operand.Type;
                    break;
                case ParameterExpression _:
                    type = expression.Type;
                    break;
            }

            if (type == null)
            {
                throw new ApplicationException(
                    $"Unable to retrieve the field type (such as return value) from expression of type {expression.GetType().Name}.");
            }

            return type;
        }

        public static string GetFieldPath(this Expression fieldSelector)
        {
            return new FieldPathVisitor(fieldSelector).GetPath();
        }
    }
}