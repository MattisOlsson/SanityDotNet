using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using SanityDotNet.QueryApi;

namespace SanityDotNet.Client
{
    public interface IQueryApi
    {
        [Get("/v1/data/query/{dataset}")]
        Task<QueryResponse<List<T>>> Query<T>(QueryParameters parameters);
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