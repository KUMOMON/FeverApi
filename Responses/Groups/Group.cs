using System.Text.Json.Serialization;

namespace FeverApi.Responses.Groups;

public class Group
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    
    [JsonPropertyName("title")]
    public string Title { get; init; }
}