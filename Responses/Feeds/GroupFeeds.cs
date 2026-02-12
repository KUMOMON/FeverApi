using System.Text.Json.Serialization;

namespace FeverApi.Responses.Feeds;

public class GroupFeeds
{
    [JsonPropertyName("group_id")]
    public int GroupId { get; init; }
    
    [JsonPropertyName("feed_ids")]
    public IReadOnlyList<int> FeedIds { get; init; }
}