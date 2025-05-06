using System.Text.Json;
using TicketsApp.Interfaces;
namespace TicketsApp.Parsers;

/// <summary>
/// Provides utility methods for parsing JSON fields using <see cref="JsonElement"/>.
/// </summary>
public class JsonParsingHelper : IJsonParsingHelper
{
    /// <summary>
    /// Retrieves the string value associated with the specified field name from a given JSON element.
    /// </summary>
    /// <param name="element">The JSON element to extract the string value from.</param>
    /// <param name="fieldName">The name of the field whose value is to be retrieved.</param>
    /// <returns>
    /// A string containing the value of the specified field, or null if the field does not exist
    /// or its value is null.
    /// </returns>
    public string? GetStringField(JsonElement element, string fieldName)
    {
        if (element.TryGetProperty(fieldName, out var value) && value.ValueKind != JsonValueKind.Null)
        {
            return value.GetString();
        }

        return null;
    }

    /// <summary>
    /// Retrieves a <see cref="DateTime"/> value from a specified field within a JSON element.
    /// </summary>
    /// <param name="element">The JSON element to search.</param>
    /// <param name="fieldName">The name of the field to retrieve the DateTime value from.</param>
    /// <returns>
    /// The <see cref="DateTime"/> value of the specified field, or <see cref="DateTime.MinValue"/>
    /// if the field is not present or the value is null.
    /// </returns>
    public DateTime GetDateTimeField(JsonElement element, string fieldName)
    {
        if (element.TryGetProperty(fieldName, out var value) && value.ValueKind != JsonValueKind.Null)
        {
            return value.GetDateTime();
        }

        return DateTime.MinValue;
    }
}