using System.Text.Json;
using TicketsApp.Models;
namespace TicketsApp.Interfaces;

public interface IErrorParser
{
    ApiErrorResponse? Parse(JsonElement element);
}