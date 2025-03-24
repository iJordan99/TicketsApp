using System.Text.Json;
using TicketsApp.Models;
namespace TicketsApp.Interfaces;

public interface ICommentParser
{
    Comment? ParseComment(JsonElement element);
}