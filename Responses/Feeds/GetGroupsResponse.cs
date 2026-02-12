using System.Text.Json.Serialization;
using FeverApi.Responses.Groups;

namespace FeverApi.Responses.Feeds;

public class GetGroupsResponse : BaseResponse
{
    [JsonPropertyName("groups")]
    public IReadOnlyList<Group> Groups { get; init; }
    
    
    [JsonPropertyName("feeds_groups")]
    public IReadOnlyList<GroupFeeds> GroupFeedsList { get; init; }
}