using System.Text.Json;
using System.Text.Json.Serialization;

namespace FeverApi.Converters;

public class BoolConverter : JsonConverter<bool>
{
    public override bool Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected a numeric boolean value");
        return reader.GetInt64() == 1L;
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        int num = value ? 1 : 0;
        writer.WriteNumberValue(num);
    }
}