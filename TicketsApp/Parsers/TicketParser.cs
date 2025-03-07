using System.Collections.ObjectModel;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Parsers;

public class TicketParser : ITicketParser
{
    public async Task<ObservableCollection<Ticket>> ParseTickets(HttpResponseMessage response,
        TicketParsingConfig config)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(json);

        var rootElement = jsonDoc.RootElement;
        if (!rootElement.TryGetProperty(config.RootProperty, out var ticketArray) ||
            ticketArray.ValueKind != JsonValueKind.Array) return new ObservableCollection<Ticket>();

        var tickets = new ObservableCollection<Ticket>();
        foreach (var ticketElement in ticketArray.EnumerateArray())
        {
            var ticket = ParseTicket(ticketElement, config);
            if (ticket != null) tickets.Add(ticket);
        }

        return tickets;
    }

    private Ticket? ParseTicket(JsonElement ticketElement, TicketParsingConfig config)
    {
        if (!ticketElement.TryGetProperty(config.FieldMappings["Id"], out var idElement) ||
            idElement.ValueKind == JsonValueKind.Null)
        {
            return null; // ID is required
        }

        var id = idElement.GetInt32();

        var attributes = string.IsNullOrEmpty(config.AttributesProperty)
            ? ticketElement
            : ticketElement.TryGetProperty(config.AttributesProperty, out var attrElement)
                ? attrElement
                : default;

        var errorCode = GetStringField(attributes, config.FieldMappings["ErrorCode"]);
        var title = GetStringField(attributes, config.FieldMappings["Title"]);
        var description = GetStringField(attributes, config.FieldMappings["Description"]);
        var status = GetStringField(attributes, config.FieldMappings["Status"]);
        var type = GetStringField(attributes, config.FieldMappings["Type"]);
        var priority = GetStringField(attributes, config.FieldMappings["Priority"]);
        var reproductionStep = GetStringField(attributes, config.FieldMappings["ReproductionStep"]);

        var createdOn = GetDateTimeField(attributes, config.FieldMappings["CreatedOn"]);
        var updatedOn = GetDateTimeField(attributes, config.FieldMappings["UpdatedOn"]);

        return new Ticket(id, errorCode, title, description, type, status, priority, reproductionStep, createdOn,
            updatedOn);
    }
    
    private DateTime GetDateTimeField(JsonElement element, string fieldName)
    {
        if (element.TryGetProperty(fieldName, out var value) && value.ValueKind != JsonValueKind.Null)
            return value.GetDateTime();
        return DateTime.MinValue;
    }

    private string? GetStringField(JsonElement element, string fieldName)
    {
        if (element.TryGetProperty(fieldName, out var value) && value.ValueKind != JsonValueKind.Null)
            return value.GetString();
        return null;
    }
}