namespace SanityDotNet.QueryApi.Projections
{
    public class ExpandProjection : Projection
    {
        private readonly string _field;

        public ExpandProjection(string field)
        {
            _field = field;
        }

        public override string ToString()
        {
            return $"{_field}->";
        }
    }
}