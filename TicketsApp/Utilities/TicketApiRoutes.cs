using TicketsApp.Models;

namespace TicketsApp.Utilities;

public static class TicketApiRoutes
{
    public static string? TicketWithIncludes(Ticket ticket, string includes)
    {
        return $"https://tickets.test/api/v1/tickets/{ticket.Id}?include={includes}";
    }

    public static string? AddComment(Ticket ticket)
    {
        return $"https://tickets.test/api/v1/tickets/{ticket.Id}/comment";
    }

    public static string? BaseTicket(Ticket ticket)
    {
        return $"https://tickets.test/api/v1/tickets/{ticket.Id}";
    }
}