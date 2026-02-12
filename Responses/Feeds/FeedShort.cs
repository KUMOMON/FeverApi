using System.Text.Json.Serialization;

namespace FeverApi.Responses.Feeds;

public class FeedShort
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    
    [JsonPropertyName("title")]
    public string Title { get; init; }
    
    [JsonPropertyName("last_updated_on_time")]
    private DateTimeOffset LastUpdatedOnTime { get; init; }
}