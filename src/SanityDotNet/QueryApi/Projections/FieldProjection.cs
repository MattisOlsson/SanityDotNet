namespace SanityDotNet.QueryApi.Projections
{
    public class FieldProjection : Projection
    {
        private readonly string _field;

        public FieldProjection(string field)
        {
            _field = field;
        }

        public override string ToString()
        {
            return _field;
        }
    }
}