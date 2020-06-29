using System;
using SanityDotNet.QueryApi.Filters;

namespace SanityDotNet.QueryApi.Projections
{
    public class DelegateProjection : Projection, IDelegateFilter<Projection>
    {
        private readonly Func<string, Projection> _projectionDelegate;

        public DelegateProjection(Func<string, Projection> projectionDelegate)
        {
            _projectionDelegate = projectionDelegate;
        }

        public Projection GetFilter(string field)
        {
            return _projectionDelegate(field);
        }
    }
}