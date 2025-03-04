using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Helpers;

public class RestApiDataParser : IRestApiDataParser
{
    public async Task<ObservableCollection<Ticket>> ParseTickets(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var ticketJsonDocumentDocument = JsonDocument.Parse(json);

        var root = ticketJsonDocumentDocument.RootElement.GetProperty("data");

        var ticketsObj = new ObservableCollection<Ticket>();

        foreach (var ticket in root.EnumerateArray())
        {
            var attributes = ticket.GetProperty("attributes");

            var ticketItem = new Ticket(
                ticket.GetProperty("id").GetInt32(),
                attributes.GetProperty("error_code").GetString(),
                attributes.GetProperty("title").GetString(),
                attributes.GetProperty("description").GetString(),
                attributes.GetProperty("status").GetString(),
                type: attributes.GetProperty("type").GetString(),
                priority: attributes.GetProperty("priority").GetString(),
                reproductionStep: attributes.GetProperty("reproduction_step").GetString(),
                createdOn: attributes.GetProperty("created_at").GetDateTime(),
                updatedOn: attributes.GetProperty("updated_at").GetDateTime()
            );
            ticketsObj.Add(ticketItem);
        }

        return ticketsObj;
    }


    public Collection<T> ParseIncludedData<T>(JsonElement root, string key, Func<JsonElement, T> parseFunc)
    {
        var collection = new Collection<T>();

        try
        {
            if (root.TryGetProperty(key, out var dataElement))
            {
                if (dataElement.ValueKind == JsonValueKind.Array)
                    foreach (var element in dataElement.EnumerateArray())
                        collection.Add(parseFunc(element));
                else
                    collection.Add(parseFunc(dataElement));
            }
        }
        catch (KeyNotFoundException)
        {
            return collection;
        }

        return collection;
    }

    public User ParseUser(JsonElement userElement)
    {
        return new User
        {
            Id = userElement.GetProperty("id").GetInt32(),
            Email = userElement.GetProperty("attributes").GetProperty("email").GetString(),
            Name = userElement.GetProperty("attributes").GetProperty("name").GetString()
        };
    }


    public Comment ParseComment(JsonElement commentElement)
    {
        var attributes = commentElement.GetProperty("attributes");
        var userElement = attributes.GetProperty("user");

        return new Comment
        {
            Id = commentElement.GetProperty("id").GetInt32(),
            Text = attributes.GetProperty("comment").GetString(),
            User = new User
            {
                Id = userElement.GetProperty("id").GetInt32(),
                Name = userElement.GetProperty("name").GetString(),
                Email = userElement.GetProperty("email").GetString(),
                IsEngineer = userElement.GetProperty("is_engineer").GetBoolean()
            }
        };
    }
}