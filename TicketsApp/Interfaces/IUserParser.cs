using System.Text.Json;
using TicketsApp.Models;
namespace TicketsApp.Interfaces;

public interface IUserParser
{

    User? ParseUser(JsonElement userElement);
}