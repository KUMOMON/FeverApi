using System.Text.Json.Serialization;

namespace FeverApi.Responses;

public class GetUnreededFeedItemsIdsResponse : BaseResponse
{
    [JsonPropertyName("unread_item_ids")]
    public IReadOnlyList<string> UnreadItemIds { get; init; }
}