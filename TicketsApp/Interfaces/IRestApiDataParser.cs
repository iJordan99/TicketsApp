using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IRestApiDataParser
{
    Task<ObservableCollection<Ticket>> ParseTickets(HttpResponseMessage response);
    Collection<T> ParseIncludedData<T>(JsonElement root, string key, Func<JsonElement, T> parseFunc);
    User ParseUser(JsonElement userElement);
    Comment ParseComment(JsonElement commentElement);
}