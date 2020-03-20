using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using SanityDotNet.Extensions;

namespace SanityDotNet.Conventions
{
    public class FieldPathVisitor : ExpressionVisitor
    {
        private readonly Expression _fieldExpression;
        private StringBuilder _path;
        private bool _inGenericDictionaryKeyExpression;

        public FieldPathVisitor(Expression fieldExpression)
        {
            _fieldExpression = fieldExpression;
        }

        public string GetPath()
        {
            _path = new StringBuilder();
            Visit(_fieldExpression);
            return _path.ToString();
        }

        private void PrependMemberName(string name)
        {
            if (_path.Length > 0)
                _path.Insert(0, ".");
            _path.Insert(0, name.ToCamelCase());
        }

        protected override Expression VisitMemberAccess(MemberExpression node)
        {
            if (!_inGenericDictionaryKeyExpression)
            {
                var aliasAsAttribute = node.Member.GetCustomAttribute<AliasAsAttribute>();
                PrependMemberName(aliasAsAttribute?.Name ?? node.Member.Name);
            }

            var expression1 = Visit(node.Expression);
            if (expression1 == node.Expression)
            {
                return node;
            }

            return node.Expression is MethodCallExpression expression2
                   && expression2.IsGetItemInvokationOnGenericDictionary()
                ? node
                : (Expression) Expression.MakeMemberAccess(expression1, node.Member);
        }

        protected override ReadOnlyCollection<Expression> VisitExpressionList(
            ReadOnlyCollection<Expression> original)
        {
            List<Expression> expressionList = null;
            var index1 = 0;

            for (var count = original.Count; index1 < count; ++index1)
            {
                var expression = Visit(original[index1]);
                if (expressionList != null)
                {
                    expressionList.Add(expression);
                }
                else if (expression != original[index1])
                {
                    expressionList = new List<Expression>(count);
                    for (var index2 = 0; index2 < index1; ++index2)
                    {
                        expressionList.Add(original[index2]);
                    }

                    expressionList.Add(expression);
                }
            }
            return expressionList != null ? expressionList.AsReadOnly() : original;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var flag = false;
            if (!_inGenericDictionaryKeyExpression && node.IsGetItemInvokationOnGenericDictionary())
            {
                flag = true;
                PrependMemberName(node.Arguments[0].CompileInvoke().ToString());
            }
            else if (!_inGenericDictionaryKeyExpression)
            {
                var aliasAsAttribute = node.Method.GetCustomAttribute<AliasAsAttribute>();
                PrependMemberName(aliasAsAttribute?.Name ?? node.Method.Name);
            }

            var methodCallExpression = node;
            var instance = Visit(node.Object);

            if (flag)
            {
                _inGenericDictionaryKeyExpression = true;
            }

            var arguments = (IEnumerable<Expression>) VisitExpressionList(node.Arguments);
            if (instance != node.Object || !Equals(arguments, node.Arguments))
            {
                methodCallExpression = Expression.Call(instance, node.Method, arguments);
            }

            if (flag)
            {
                _inGenericDictionaryKeyExpression = false;
            }

            return methodCallExpression;
        }
    }
}