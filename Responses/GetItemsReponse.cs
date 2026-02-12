using System.Text.Json.Serialization;
using FeverApi.Responses.FeedItems;

namespace FeverApi.Responses;

public class GetItemsResponse<T> : BaseResponse where T : ItemShort
{
    [JsonPropertyName("total_items")]
    public int TotalItems { get; init; }

    [JsonPropertyName("items")]
    public IReadOnlyList<T>? Items { get; init; }
}