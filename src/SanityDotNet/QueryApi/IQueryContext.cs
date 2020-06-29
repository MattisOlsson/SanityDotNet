using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;

namespace SanityDotNet.QueryApi
{
    public interface IQueryContext
    {
        IQueryBuilder Query { get; set; }
        List<Expression> Projections { get; set; }
    }

    public class QueryContext : IQueryContext, IDisposable
    {
        private readonly CultureInfo _previousUiCulture;

        public QueryContext(IQueryBuilder query)
        {
            _previousUiCulture = Thread.CurrentThread.CurrentUICulture;
            Query = query;
            Projections = new List<Expression>();
            Thread.CurrentThread.CurrentUICulture = query.Language;
        }

        public IQueryBuilder Query { get; set; }

        public List<Expression> Projections { get; set; }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentUICulture = _previousUiCulture;
        }
    }
}