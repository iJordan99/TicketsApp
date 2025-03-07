using System.Globalization;
using TicketsApp.Models;

namespace TicketsApp.Converters;

public class TicketFormatConverter : IValueConverter
{
    private static readonly Dictionary<string?, string> StatusMappings = new()
    {
        { "A", "Active" },
        { "C", "Closed" },
        { "H", "Hold" },
        { "X", "Cancelled" }
    };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Ticket ticket)
        {
            var statusText = StatusMappings.TryGetValue(ticket.Status, out var fullStatus) ? fullStatus : ticket.Status;
            return $"[ #{ticket.Id} | {statusText} | {ticket.Priority} | {ticket.Type}] : {ticket.Title}";
        }

        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}