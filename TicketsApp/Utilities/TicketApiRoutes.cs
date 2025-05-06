namespace TicketsApp.Utilities;

public static class TicketApiRoutes
{
    public static string? GetTicketWithIncludesUri(int ticketId, string includes)
    {
        return $"https://tickets.test/api/v1/tickets/{ticketId}?include={includes}";
    }

    public static string? AddComment(int ticketId)
    {
        return $"https://tickets.test/api/v1/tickets/{ticketId}/comment";
    }
}