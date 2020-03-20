using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SanityDotNet
{
    public class ExpressionFinder<TExpression> : ExpressionVisitor where TExpression : Expression
    {
        private readonly IList<TExpression> _result = new List<TExpression>();
        private Func<TExpression, bool> _predicate;

        /// <summary>
        /// Returns a list of <typeparamref name="TExpression" /> instances that matches the <paramref name="predicate" />.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the sub tree for which to start searching.</param>
        /// <param name="predicate">The <see cref="T:System.Func`2" /> used to filter the result</param>
        /// <returns>A list of <see cref="T:System.Linq.Expressions.Expression" /> instances that matches the given predicate.</returns>
        public IEnumerable<TExpression> Find(
            Expression expression,
            Func<TExpression, bool> predicate)
        {
            _result.Clear();
            _predicate = predicate;
            Visit(expression);
            return _result;
        }

        /// <summary>
        /// Visits each node of the <see cref="T:System.Linq.Expressions.Expression" /> tree checks
        /// if the current expression matches the predicate.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.Linq.Expressions.Expression" /> currently being visited.</param>
        /// <returns><see cref="T:System.Linq.Expressions.Expression" /></returns>
        protected override Expression Visit(Expression expression)
        {
            if (expression != null && expression is TExpression expression1 && _predicate(expression1))
            {
                _result.Add(expression1);
            }

            return base.Visit(expression);
        }
    }
}