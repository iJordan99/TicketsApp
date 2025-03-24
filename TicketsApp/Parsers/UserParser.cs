using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Parsers;

public class UserParser(IJsonParsingHelper jsonHelper) : IUserParser
{
    public User? ParseUser(JsonElement element)
    {
        if (!element.TryGetProperty("id", out var idElement) || idElement.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var attributes = element.TryGetProperty("attributes", out var attrElement) ? attrElement : element;

        return new User
        {
            Id = idElement.GetInt32(),
            Name = jsonHelper.GetStringField(attributes, "name"),
            Email = jsonHelper.GetStringField(attributes, "email"),
            IsEngineer = attributes.TryGetProperty("is_engineer", out var isEng) && isEng.GetBoolean()
        };
    }
}