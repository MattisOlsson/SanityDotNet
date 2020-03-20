using System;
using System.Linq.Expressions;

namespace SanityDotNet
{
    public class ExpressionReplacer<TExpression> : ExpressionVisitor where TExpression : Expression
    {
        private Func<TExpression, Expression> _replaceWith;
        private Func<TExpression, bool> _predicate;

        /// <summary>
        /// Searches for expressions using the given <paramref name="predicate" /> and
        /// replaces matches with the result from the <paramref name="replaceWith" /> delegate.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.Linq.Expressions.Expression" /> that
        /// represents the sub tree for which to start searching.</param>
        /// <param name="predicate">The <see cref="T:System.Func`2" /> used to filter the result</param>
        /// <param name="replaceWith">The <see cref="T:System.Func`2" />
        /// used to specify the replacement expression.</param>
        /// <returns>The modified <see cref="T:System.Linq.Expressions.Expression" /> tree.</returns>
        public Expression Replace(
            Expression expression,
            Func<TExpression, bool> predicate,
            Func<TExpression, Expression> replaceWith)
        {
            _replaceWith = replaceWith;
            _predicate = predicate;
            return Visit(expression);
        }

        /// <summary>
        /// Visits each node of the <see cref="T:System.Linq.Expressions.Expression" /> tree checks
        /// if the current expression matches the predicate. If a match is found
        /// the expression will be replaced.
        /// </summary>
        /// <param name="expression">The <see cref="T:System.Linq.Expressions.Expression" /> currently being visited.</param>
        /// <returns><see cref="T:System.Linq.Expressions.Expression" /></returns>
        protected override Expression Visit(Expression expression)
        {
            return expression != null && expression is TExpression expression1 && _predicate(expression1)
                ? _replaceWith((TExpression) expression)
                : base.Visit(expression);
        }
    }
}