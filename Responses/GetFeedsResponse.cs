using System.Text.Json.Serialization;
using FeverApi.Responses.Feeds;

namespace FeverApi.Responses;

public class GetFeedsResponse<T> : BaseResponse where T : FeedShort
{
    [JsonPropertyName("feeds")]
    public IReadOnlyList<T> Feeds { get; init; }
    
    [JsonPropertyName("feeds_groups")]
    public IReadOnlyList<GroupFeeds> GroupFeedsList { get; init; }
}