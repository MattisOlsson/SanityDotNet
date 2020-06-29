using SanityDotNet.Models.FieldTypes;

namespace SanityDotNet.QueryApi.Projections
{
    public static class ProjectionExtensions
    {
        public static DelegateProjection Expand(this Reference reference)
        {
            return new DelegateProjection(field => new ExpandProjection(field));
        }

        public static DelegateProjection Project(this object value)
        {
            return new DelegateProjection(field => new FieldProjection(field));
        }
    }
}