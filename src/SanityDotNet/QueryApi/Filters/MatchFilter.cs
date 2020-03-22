namespace SanityDotNet.QueryApi.Filters
{
    public class MatchFilter : Filter
    {
        public string Field { get; }
        public FieldFilterValue ValueToMatch { get; }

        public MatchFilter(string field, FieldFilterValue valueToMatch)
        {
            Field = field;
            ValueToMatch = valueToMatch;
        }

        public override string ToString()
        {
            return $"{Field} match {ValueToMatch.ToQueryFormat()}";
        }
    }
}