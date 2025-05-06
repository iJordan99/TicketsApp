using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Parsers;

/// <summary>
/// Parses JSON data to create instances of the <c>User</c> model.
/// </summary>
/// <remarks>
/// This class implements the <c>IUserParser</c> interface and provides functionality to parse
/// JSON elements and map them to <c>User</c> objects. The parsing process handles the extraction
/// of relevant fields such as "id", "email", "name", and "is_engineer" from the JSON structure.
/// </remarks>
/// <param name="jsonHelper">
/// An instance of <c>IJsonParsingHelper</c> used to assist with JSON field extraction.
/// </param>
public class UserParser(IJsonParsingHelper jsonHelper) : IUserParser
{
    /// Parses a JSON element into a User object.
    /// <param name="element">
    /// The JSON element containing the user data to be parsed.
    /// </param>
    /// <returns>
    /// A User object if parsing is successful; otherwise, null.
    /// </returns>
    public User? Parse(JsonElement element)
    {
        if (!element.TryGetProperty("id", out var idElement) || idElement.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var attributes = element.TryGetProperty("attributes", out var attrElement) ? attrElement : element;

        return new User
        (
            jsonHelper.GetStringField(attributes, "email"),
            idElement.GetInt32(),
            attributes.TryGetProperty("is_engineer", out var isEng),
            jsonHelper.GetStringField(attributes, "name")
        );
    }
}