using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Parsers;

public class CommentParser(IUserParser userParser, IJsonParsingHelper jsonHelper) : ICommentParser
{
    public Comment? ParseComment(JsonElement element)
    {
        if (!element.TryGetProperty("id", out var idElement) || idElement.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var attributes = element.TryGetProperty("attributes", out var attrElement) ? attrElement : element;

        User? user = null;
        if (attributes.TryGetProperty("user", out var userElement))
        {
            user = userParser.ParseUser(userElement);
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