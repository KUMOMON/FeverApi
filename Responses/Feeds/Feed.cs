using System.Text.Json.Serialization;

namespace FeverApi.Responses.Feeds;

/// <remarks>field `is_spark` unsported and always 0(false)</remarks>
public class Feed : FeedShort
{
    [JsonPropertyName("favicon_id")]
    public int FaviconId { get; init; }
    
    /// <summary>
    /// url to rss source
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; init; }
    
    /// <summary>
    /// Site url
    /// </summary>
    [JsonPropertyName("site_url")]
    public string SiteUrl { get; init; }
}