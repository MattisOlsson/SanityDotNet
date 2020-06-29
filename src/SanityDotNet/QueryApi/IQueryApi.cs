using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace SanityDotNet.QueryApi
{
    public interface IQueryApi
    {
        [Get("/data/query/{dataset}?query={query}")]
        Task<QueryResponse<T>> Query<T>(string dataset, string query);
    }

    //
    // public class SanityQueryBuilderConverter : JsonConverter<ISanityQueryBuilder>
    // {
    //     public override void WriteJson(JsonWriter writer, ISanityQueryBuilder value, JsonSerializer serializer)
    //     {
    //         var encodedString = value?.ToEncodedString();
    //
    //         if (!string.IsNullOrEmpty(encodedString))
    //         {
    //             writer.WriteValue(encodedString);
    //         }
    //     }
    //
    //     public override ISanityQueryBuilder ReadJson(
    //         JsonReader reader,
    //         Type objectType,
    //         ISanityQueryBuilder existingValue,
    //         bool hasExistingValue,
    //         JsonSerializer serializer)
    //     {
    //         var value = reader.Value as string;
    //
    //         return new SanityQueryBuilder(value);
    //     }
    // }
}