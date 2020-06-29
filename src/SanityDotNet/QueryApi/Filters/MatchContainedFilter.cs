namespace SanityDotNet.QueryApi.Filters
{
    public class MatchContainedFilter : Filter
    {
        public string Field { get; }
        public FieldFilterValue ValueToMatch { get; }
        public string FieldSuffix { get; set; }

        public MatchContainedFilter(string field, FieldFilterValue valueToMatch)
        {
            Field = field;
            ValueToMatch = valueToMatch;
            FieldSuffix = string.Empty;
        }

        public override string ToString()
        {
            return $"{ValueToMatch.ToQueryFormat()} in {Field}{FieldSuffix}";
        }
    }
}