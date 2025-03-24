using System.Text.Json;
using TicketsApp.Interfaces;
namespace TicketsApp.Parsers;

public class JsonParsingHelper : IJsonParsingHelper
{
    // Tries to get a string value from a JSON element by field name. 
    // Returns the string if found and not null, otherwise returns null.
    public string? GetStringField(JsonElement element, string fieldName)
    {
        if (element.TryGetProperty(fieldName, out var value) && value.ValueKind != JsonValueKind.Null)
        {
            return value.GetString();
        }

        return null;
    }

    // Tries to get a DateTime value from a JSON element by field name. 
    // Returns the DateTime if found and not null, otherwise returns DateTime.MinValue.
    public DateTime GetDateTimeField(JsonElement element, string fieldName)
    {
        if (element.TryGetProperty(fieldName, out var value) && value.ValueKind != JsonValueKind.Null)
        {
            return value.GetDateTime();
        }

        return DateTime.MinValue;
    }
}