using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Parsers;

/// <summary>
/// Parses a JSON representation of a comment into a <see cref="Comment"/> object.
/// </summary>
public class CommentParser(IUserParser userParser, IJsonParsingHelper jsonHelper) : ICommentParser
{
    /// Parses a JSON element into a Comment object.
    /// <param name="element">
    /// A JsonElement representing a comment to be parsed. Must contain an "id" property,
    /// and may contain optional fields such as "attributes", "comment", "user", and "created_at".
    /// </param>
    /// <returns>
    /// A Comment object populated with the extracted data if parsing is successful; otherwise, null.
    /// </returns>
    public Comment? Parse(JsonElement element)
    {
        if (!element.TryGetProperty("id", out var idElement) || idElement.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var attributes = element.TryGetProperty("attributes", out var attrElement) ? attrElement : element;

        User? user = null;
        if (attributes.TryGetProperty("user", out var userElement))
        {
            user = userParser.Parse(userElement);
        }

        return new Comment
        {
            Id = idElement.GetInt32(),
            Text = jsonHelper.GetStringField(attributes, "comment"),
            User = user,
            Date = jsonHelper.GetDateTimeField(attributes, "created_at")
        };
    }
}