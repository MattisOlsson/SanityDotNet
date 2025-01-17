using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using SanityDotNet.Reflection;

namespace SanityDotNet.Expressions
{
    public abstract class ExpressionVisitor
    {
        protected virtual Expression Visit(Expression exp)
        {
            if (exp == null)
                return null;
            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return VisitBinary((BinaryExpression) exp);
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return VisitUnary((UnaryExpression) exp);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression) exp);
                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpression) exp);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression) exp);
                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpression) exp);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression) exp);
                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpression) exp);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression) exp);
                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpression) exp);
                case ExpressionType.New:
                    return VisitNew((NewExpression) exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return VisitNewArray((NewArrayExpression) exp);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression) exp);
                case ExpressionType.TypeIs:
                    return VisitTypeIs((TypeBinaryExpression) exp);
                default:
                    throw new Exception($"Unhandled expression type: '{(object) exp.NodeType}'");
            }
        }

        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return VisitMemberAssignment((MemberAssignment) binding);
                case MemberBindingType.MemberBinding:
                    return VisitMemberMemberBinding((MemberMemberBinding) binding);
                case MemberBindingType.ListBinding:
                    return VisitMemberListBinding((MemberListBinding) binding);
                default:
                    throw new Exception($"Unhandled binding type '{(object) binding.BindingType}'");
            }
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            var readOnlyCollection = VisitExpressionList(initializer.Arguments);
            return readOnlyCollection != initializer.Arguments
                ? Expression.ElementInit(initializer.AddMethod, readOnlyCollection)
                : initializer;
        }

        protected virtual Expression VisitUnary(UnaryExpression u)
        {
            var operand = Visit(u.Operand);
            return operand != u.Operand ? Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method) : (Expression) u;
        }

        protected virtual Expression VisitBinary(BinaryExpression b)
        {
            var left = Visit(b.Left);
            var right = Visit(b.Right);
            var expression = Visit(b.Conversion);
            if (left == b.Left && right == b.Right && expression == b.Conversion)
                return b;
            return b.NodeType == ExpressionType.Coalesce && b.Conversion != null
                ? Expression.Coalesce(left, right, expression as LambdaExpression)
                : (Expression) Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
        {
            var expression = Visit(b.Expression);
            return expression != b.Expression ? Expression.TypeIs(expression, b.TypeOperand) : (Expression) b;
        }

        protected virtual Expression VisitConstant(ConstantExpression c)
        {
            return c;
        }

        protected virtual Expression VisitConditional(ConditionalExpression c)
        {
            var test = Visit(c.Test);
            var ifTrue = Visit(c.IfTrue);
            var ifFalse = Visit(c.IfFalse);
            return test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse
                ? Expression.Condition(test, ifTrue, ifFalse)
                : (Expression) c;
        }

        protected virtual Expression VisitParameter(ParameterExpression p)
        {
            return p;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression m)
        {
            var expression = Visit(m.Expression);
            return expression != m.Expression ? Expression.MakeMemberAccess(expression, m.Member) : (Expression) m;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression m)
        {
            var instance = Visit(m.Object);
            IEnumerable<Expression> arguments = VisitExpressionList(m.Arguments);
            return instance != m.Object || arguments != m.Arguments
                ? Expression.Call(instance, m.Method, arguments)
                : (Expression) m;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(
            ReadOnlyCollection<Expression> original)
        {
            List<Expression> expressionList = null;
            var index1 = 0;
            for (var count = original.Count; index1 < count; ++index1)
            {
                var expression = Visit(original[index1]);
                if (expressionList != null)
                    expressionList.Add(expression);
                else if (expression != original[index1])
                {
                    expressionList = new List<Expression>(count);
                    for (var index2 = 0; index2 < index1; ++index2)
                        expressionList.Add(original[index2]);
                    expressionList.Add(expression);
                }
            }

            return expressionList != null ? expressionList.AsReadOnly() : original;
        }

        protected virtual MemberAssignment VisitMemberAssignment(
            MemberAssignment assignment)
        {
            var expression = Visit(assignment.Expression);
            if (expression == assignment.Expression)
                return assignment;
            try
            {
                return Expression.Bind(assignment.Member, expression);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(
                    $"Argument types do not match for member {(object) assignment.Member.Name} of type {(object) assignment.Member.DeclaringType}, expected {(object) assignment.Member.GetMemberReturnType().Name} but argument {(object) assignment.Expression} is {(object) expression.Type}.",
                    ex.ParamName, ex);
            }
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(
            MemberMemberBinding binding)
        {
            var bindings = VisitBindingList(binding.Bindings);
            return bindings != binding.Bindings ? Expression.MemberBind(binding.Member, bindings) : binding;
        }

        protected virtual MemberListBinding VisitMemberListBinding(
            MemberListBinding binding)
        {
            var initializers = VisitElementInitializerList(binding.Initializers);
            return initializers != binding.Initializers ? Expression.ListBind(binding.Member, initializers) : binding;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(
            ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> memberBindingList = null;
            var index1 = 0;
            for (var count = original.Count; index1 < count; ++index1)
            {
                var memberBinding = VisitBinding(original[index1]);
                if (memberBindingList != null)
                    memberBindingList.Add(memberBinding);
                else if (memberBinding != original[index1])
                {
                    memberBindingList = new List<MemberBinding>(count);
                    for (var index2 = 0; index2 < index1; ++index2)
                        memberBindingList.Add(original[index2]);
                    memberBindingList.Add(memberBinding);
                }
            }

            return memberBindingList ?? (IEnumerable<MemberBinding>) original;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(
            ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> elementInitList = null;
            var index1 = 0;
            for (var count = original.Count; index1 < count; ++index1)
            {
                var elementInit = VisitElementInitializer(original[index1]);
                if (elementInitList != null)
                    elementInitList.Add(elementInit);
                else if (elementInit != original[index1])
                {
                    elementInitList = new List<ElementInit>(count);
                    for (var index2 = 0; index2 < index1; ++index2)
                        elementInitList.Add(original[index2]);
                    elementInitList.Add(elementInit);
                }
            }

            return elementInitList ?? (IEnumerable<ElementInit>) original;
        }

        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            var body = Visit(lambda.Body);
            return body != lambda.Body ? Expression.Lambda(lambda.Type, body, lambda.Parameters) : (Expression) lambda;
        }

        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> arguments = VisitExpressionList(nex.Arguments);
            if (arguments == nex.Arguments)
                return nex;
            return nex.Members != null
                ? Expression.New(nex.Constructor, arguments, nex.Members)
                : Expression.New(nex.Constructor, arguments);
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            var newExpression = VisitNew(init.NewExpression);
            var bindings = VisitBindingList(init.Bindings);
            return newExpression != init.NewExpression || bindings != init.Bindings
                ? Expression.MemberInit(newExpression, bindings)
                : (Expression) init;
        }

        protected virtual Expression VisitListInit(ListInitExpression init)
        {
            var newExpression = VisitNew(init.NewExpression);
            var initializers = VisitElementInitializerList(init.Initializers);
            return newExpression != init.NewExpression || initializers != init.Initializers
                ? Expression.ListInit(newExpression, initializers)
                : (Expression) init;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression na)
        {
            IEnumerable<Expression> expressions = VisitExpressionList(na.Expressions);
            if (expressions == na.Expressions)
                return na;
            return na.NodeType == ExpressionType.NewArrayInit
                ? Expression.NewArrayInit(na.Type.GetElementType(), expressions)
                : (Expression) Expression.NewArrayBounds(na.Type.GetElementType(), expressions);
        }

        protected virtual Expression VisitInvocation(InvocationExpression iv)
        {
            IEnumerable<Expression> arguments = VisitExpressionList(iv.Arguments);
            var expression = Visit(iv.Expression);
            return arguments != iv.Arguments || expression != iv.Expression
                ? Expression.Invoke(expression, arguments)
                : (Expression) iv;
        }
    }
}