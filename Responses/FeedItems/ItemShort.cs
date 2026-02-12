using System.Text.Json.Serialization;

namespace FeverApi.Responses.FeedItems;

public class ItemShort
{
    [JsonPropertyName("id")]
    public string Id { get; init; }
    
    [JsonPropertyName("feed_id")]
    public int FeedId { get; init; }
    
    [JsonPropertyName("title")]
    public string Title { get; init; }
    
    [JsonPropertyName("created_on_time")]
    private DateTimeOffset CreatedOnTime { get; init; }
    
    [JsonPropertyName("is_read")]
    public bool IsRead { get; init; }
}