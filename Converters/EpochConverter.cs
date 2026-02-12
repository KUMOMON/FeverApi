using System.Text.Json;
using System.Text.Json.Serialization;

namespace FeverApi.Converters;

public class EpochConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Number ? DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()) : throw new JsonException("Expected a numeric epoch timestamp");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeOffset value,
        JsonSerializerOptions options)
    {
        long unixTimeSeconds = value.ToUnixTimeSeconds();
        writer.WriteNumberValue(unixTimeSeconds);
    }
}