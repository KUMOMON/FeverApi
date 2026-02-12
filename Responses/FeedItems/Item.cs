using System.Text.Json.Serialization;

namespace FeverApi.Responses.FeedItems;

public class Item : ItemShort
{
    [JsonPropertyName("author")]
    public string Author { get; init; }    
    
    [JsonPropertyName("html")]
    public string Html { get; init; }
    
    [JsonPropertyName("url")]
    public string Url { get; init; }
    
    [JsonPropertyName("is_saved")]
    public bool IsSaved { get; init; }
}