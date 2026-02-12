using System.Text.Json;
using System.Text.Json.Serialization;

namespace FeverApi.Converters;

public class CommaSeparatedListIntsConverter: JsonConverter<IReadOnlyList<int>>
{
    public override IReadOnlyList<int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string");
        }

        var commaSeparatedList = reader.GetString() ?? string.Empty;

        return commaSeparatedList.Split(",",StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(Int32.Parse).ToArray();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<int> value, JsonSerializerOptions options)
    {
        var commaSeparatedList = string.Join(",", value);
        writer.WriteStringValue(commaSeparatedList);
    }
}