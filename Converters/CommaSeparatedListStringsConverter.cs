using System.Text.Json;
using System.Text.Json.Serialization;

namespace FeverApi.Converters;

public class CommaSeparatedListStringsConverter : JsonConverter<IReadOnlyList<string>>
{
    public override IReadOnlyList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string");
        }

        var commaSeparatedList = reader.GetString() ?? string.Empty;

        return commaSeparatedList.Split(",",StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<string> value, JsonSerializerOptions options)
    {
        var commaSeparatedList = string.Join(",", value);
        writer.WriteStringValue(commaSeparatedList);
    }
}