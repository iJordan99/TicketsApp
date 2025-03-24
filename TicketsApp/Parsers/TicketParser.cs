using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Parsers;

public class TicketParser(IJsonParsingHelper jsonHelper, TicketParsingConfig config, IUserParser userParser, ICommentParser commentParser) : ITicketParser
{

    public async Task<ObservableCollection<Ticket>> ParseTickets(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(json);

        var rootElement = jsonDoc.RootElement;

        // Checks if the JSON has a ticket list in the expected section
        if (!rootElement.TryGetProperty(config.RootProperty, out var ticketArray) || ticketArray.ValueKind != JsonValueKind.Array)
        {
            return new ObservableCollection<Ticket>();
        }

        //If there are a list of tickets, parse each and return ticket collection.
        var tickets = new ObservableCollection<Ticket>();
        foreach (var ticketElement in ticketArray.EnumerateArray())
        {
            var ticket = ParseTicket(ticketElement);
            if (ticket != null)
            {
                tickets.Add(ticket);
            }
        }

        return tickets;
    }

    public async Task<TicketWithIncludes?> ParseTicketWithIncludes(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(json);
        var rootElement = jsonDoc.RootElement;

        // Initialize default values
        User? author = null;
        var engineers = new ObservableCollection<User>();
        var comments = new ObservableCollection<Comment>();
        Ticket? ticket = null;

        // Parse the ticket from the "data" property
        if (rootElement.TryGetProperty(config.RootProperty, out var dataElement) &&
            dataElement.ValueKind == JsonValueKind.Object)
        {
            // Parse the ticket
            ticket = ParseTicket(dataElement);

            if (dataElement.TryGetProperty(config.IncludesProperty, out var includesElement) &&
                includesElement.ValueKind == JsonValueKind.Object)
            {
                // Parse author
                if (includesElement.TryGetProperty(config.IncludesMappings["Author"], out var authorElement))
                {
                    author = userParser.ParseUser(authorElement);
                }

                // Parse engineers
                if (includesElement.TryGetProperty(config.IncludesMappings["Engineer"], out var engineerElement) &&
                    engineerElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var engineer in engineerElement.EnumerateArray())
                    {
                        var user = userParser.ParseUser(engineer);
                        if (user != null)
                        {
                            engineers.Add(user);
                        }
                    }
                }

                // Parse comments
                if (includesElement.TryGetProperty(config.IncludesMappings["Comments"], out var commentsElement) &&
                    commentsElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var comment in commentsElement.EnumerateArray())
                    {
                        var parsedComment = commentParser.ParseComment(comment);
                        if (parsedComment != null)
                        {
                            comments.Add(parsedComment);
                        }
                    }
                }
            }
        }

        return ticket != null ? new TicketWithIncludes(ticket, engineers, author, comments) : null;
    }


    private Ticket? ParseTicket(JsonElement ticketElement)
    {
        if (!ticketElement.TryGetProperty(config.FieldMappings["Id"], out var idElement) || idElement.ValueKind == JsonValueKind.Null)
        {
            return null; // ID is required
        }

        var id = idElement.GetInt32();

        var attributes = string.IsNullOrEmpty(config.AttributesProperty)
            ? ticketElement
            : ticketElement.TryGetProperty(config.AttributesProperty, out var attrElement)
                ? attrElement
                : default;

        var errorCode = jsonHelper.GetStringField(attributes, config.FieldMappings["ErrorCode"]);
        var title = jsonHelper.GetStringField(attributes, config.FieldMappings["Title"]);
        var description = jsonHelper.GetStringField(attributes, config.FieldMappings["Description"]);
        var status = jsonHelper.GetStringField(attributes, config.FieldMappings["Status"]);
        var type = jsonHelper.GetStringField(attributes, config.FieldMappings["Type"]);
        var priority = jsonHelper.GetStringField(attributes, config.FieldMappings["Priority"]);
        var reproductionStep = jsonHelper.GetStringField(attributes, config.FieldMappings["ReproductionStep"]);

        var createdOn = jsonHelper.GetDateTimeField(attributes, config.FieldMappings["CreatedOn"]);
        var updatedOn = jsonHelper.GetDateTimeField(attributes, config.FieldMappings["UpdatedOn"]);

        return new Ticket(id, errorCode, title, description, type, status, priority, reproductionStep, createdOn,
            updatedOn);
    }
}