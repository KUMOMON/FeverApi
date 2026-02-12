using System.Text.Json.Serialization;

namespace FeverApi.Responses;

public class BaseResponse
{
    [JsonPropertyName("api_version")]
    public int ApiVersion { get; init; }

    [JsonPropertyName("auth")]
    public bool Auth { get; init; }

    [JsonPropertyName("last_refreshed_on_time")]
    public DateTimeOffset LastRefreshedOnTime { get; init; }
}