using System.Text.Json;
namespace TicketsApp.Interfaces;

public interface IJsonParsingHelper
{
    string? GetStringField(JsonElement element, string fieldName);
    DateTime GetDateTimeField(JsonElement element, string fieldName);
}