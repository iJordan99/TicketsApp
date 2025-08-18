using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IUserParser
{
    User? Parse(JsonElement userElement);
    Task<ObservableCollection<User>> ParseMany(HttpResponseMessage response);
}